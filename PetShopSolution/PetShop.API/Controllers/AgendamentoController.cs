using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using PetShop.Application.DTOs;
using PetShop.Application.Enums;
using PetShop.Application.Services;

namespace PetShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController : ControllerBase
{
    private readonly AgendamentoService _service;
    private readonly AtendimentoService _atendimentoService;
    private readonly DisponibilidadeService _disponibilidadeService;

    public AgendamentoController(
        AgendamentoService service,
        AtendimentoService atendimentoService,
        DisponibilidadeService disponibilidadeService)
    {
        _service = service;
        _atendimentoService = atendimentoService;
        this._disponibilidadeService = disponibilidadeService;
    }

    /// <summary>
    /// Retorna um agendamento específico pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var agendamento = await _service.GetById(id, cancellationToken);
        if (agendamento == null)
            return NotFound(new { message = "Agendamento não encontrado." });

        return Ok(agendamento);
    }
    
    [HttpGet("disponibilidades/{data}")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetHorariosDisponiveisPorDia(string data, CancellationToken cancellationToken)
    {
        if (!DateTime.TryParse(data, out var dataConsulta))
            return BadRequest(new { message = "Formato de data inválido. Use yyyy-MM-dd." });

        // 1️⃣ Se for domingo, não há expediente
        if (dataConsulta.DayOfWeek == DayOfWeek.Sunday)
            return Ok(new List<string>());

        // 2️⃣ Se o dia estiver bloqueado manualmente (indisponível)
        var diasIndisponiveis = await _disponibilidadeService.GetIndisponiveis(cancellationToken);
        if (diasIndisponiveis.Any(d => d.Data.Date == dataConsulta.Date))
            return Ok(new List<string>());

        // 3️⃣ Monta lista base de horários (08:00 → 17:00, intervalos de 1h)
        var inicioExpediente = new TimeSpan(8, 0, 0);
        var fimExpediente = new TimeSpan(17, 0, 0);
        var duracaoConsulta = TimeSpan.FromHours(1);

        var horarios = new List<string>();
        for (var hora = inicioExpediente; hora < fimExpediente; hora += duracaoConsulta)
            horarios.Add(hora.ToString(@"hh\:mm"));

        // 4️⃣ Busca agendamentos já existentes no banco para esse dia
        var agendamentos = await _service.GetByDate(dataConsulta, cancellationToken);

        // 5️⃣ Remove da lista os horários já ocupados
        foreach (var agendamento in agendamentos)
        {
            if (agendamento.dataConsulta.HasValue)
            {
                var horaMarcada = agendamento.dataConsulta.Value.ToLocalTime().ToString("HH:mm");
                horarios.Remove(horaMarcada);
            }
        }

        // 6️⃣ Retorna lista final de horários livres
        return Ok(horarios);
    }

    // ===========================================================
    // 🔹 ROTA 2: Marcar dia como indisponível
    // ===========================================================
    [HttpPost("indisponiveis")]
    public async Task<IActionResult> PostDiaIndisponivel([FromBody] DiasIndisponiveis dia, CancellationToken cancellationToken)
    {
        try
        {
            await _disponibilidadeService.AddIndisponivel(dia, cancellationToken);
            return Ok(new { message = $"Dia {dia.Data:dd/MM/yyyy} marcado como indisponível." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ===========================================================
    // 🔹 ROTA 3 (Opcional): Remover dia da lista de indisponíveis
    // ===========================================================
    [HttpDelete("indisponiveis/{data}")]
    public async Task<IActionResult> DeleteDiaIndisponivel(string data, CancellationToken cancellationToken)
    {
        if (!DateTime.TryParse(data, out var parsed))
            return BadRequest(new { message = "Formato de data inválido. Use yyyy-MM-dd." });

        await _disponibilidadeService.RemoverIndisponivel(parsed, cancellationToken);
        return Ok(new { message = $"Dia {parsed:dd/MM/yyyy} removido da lista de indisponíveis." });
    }
    
    /// <summary>
    /// Retorna todos os dias indisponíveis do calendário, com seus respectivos horários ocupados.
    /// Inclui tanto dias bloqueados manualmente quanto dias parcialmente ocupados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de dias indisponíveis com detalhes de horários ocupados</returns>
    [HttpGet("indisponibilidades-detalhadas")]
    [ProducesResponseType(typeof(List<DisponibilidadeDiaDTO>), 200)]
    public async Task<IActionResult> GetDiasComHorariosIndisponiveis(CancellationToken cancellationToken)
    {
        var indisponiveis = await _disponibilidadeService.GetIndisponiveis(cancellationToken);
        var diasIndisponiveis = indisponiveis.Select(d => d.Data.Date).ToHashSet();

        var hoje = DateTime.Today;
        var lista = new List<DisponibilidadeDiaDTO>();

        // Avalia os próximos 30 dias
        for (int i = 0; i < 30; i++)
        {
            var dia = hoje.AddDays(i);

            // Ignora domingos (sem expediente)
            if (dia.DayOfWeek == DayOfWeek.Sunday)
                continue;

            // Se o dia está marcado como totalmente indisponível
            if (diasIndisponiveis.Contains(dia.Date))
            {
                lista.Add(new DisponibilidadeDiaDTO
                {
                    Data = dia.ToString("yyyy-MM-dd"),
                    HorariosDisponiveis = new List<string>() // nenhum horário disponível
                });
                continue;
            }

            // Caso não esteja bloqueado, buscamos horários já ocupados
            var horariosOcupados = await GetHorariosIndisponiveisInterno(dia, cancellationToken);

            // Se o dia estiver parcialmente indisponível (alguns horários ocupados)
            if (horariosOcupados.Any())
            {
                lista.Add(new DisponibilidadeDiaDTO
                {
                    Data = dia.ToString("yyyy-MM-dd"),
                    HorariosDisponiveis = horariosOcupados
                });
            }
        }

        return Ok(lista);
    }
    
    // ===========================================================
    // 🔹 NOVA ROTA: Retorna dias + horários Indisponíveis
    // ===========================================================
    /// <summary>
    /// Retorna todos os dias disponíveis do calendário com seus respectivos horários disponíveis.
    /// </summary>
    // 🔹 Retorna os horários disponíveis por dia
    [HttpGet("disponibilidades-detalhadas")]
    public async Task<IActionResult> GetDiasComHorariosDisponiveis(CancellationToken cancellationToken)
    {
        var indisponiveis = await _disponibilidadeService.GetIndisponiveis(cancellationToken);
        var diasIndisponiveis = indisponiveis.Select(d => d.Data.Date).ToHashSet();

        var hoje = DateTime.Today;
        var lista = new List<DisponibilidadeDiaDTO>();

        for (int i = 0; i < 30; i++)
        {
            var dia = hoje.AddDays(i);

            if (dia.DayOfWeek == DayOfWeek.Sunday)
            {
                diasIndisponiveis.Add(dia.Date);
                continue;
            }

            if (diasIndisponiveis.Contains(dia.Date))
                continue;

            var horariosLivres = await GetHorariosDisponiveisInterno(dia, cancellationToken);

            if (horariosLivres.Any())
            {
                lista.Add(new DisponibilidadeDiaDTO
                {
                    Data = dia.ToString("yyyy-MM-dd"),
                    HorariosDisponiveis = horariosLivres
                });
            }
        }

        return Ok(lista);
    }
    
    /// <summary>
    /// Retorna todos os agendamentos de um veterinário específico.
    /// </summary>
    /// <param name="veterinarioId">ID do veterinário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de agendamentos do veterinário</returns>
    [HttpGet("veterinario/{veterinarioId}")]
    public async Task<IActionResult> GetByVeterinario(string veterinarioId, CancellationToken cancellationToken)
    {
        var lista = await _service.GetByVeterinario(veterinarioId, cancellationToken);
    
        if (!lista.Any())
            return NotFound(new { message = "Nenhum agendamento encontrado para este veterinário." });

        return Ok(lista);
    }
    
    /// <summary>
    /// Retorna a lista de horários ocupados (indisponíveis) em um determinado dia.
    /// </summary>
    private async Task<List<string>> GetHorariosIndisponiveisInterno(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var agendamentos = await _disponibilidadeService.GetByDate(dataConsulta, cancellationToken);

        // Extrai apenas o horário (HH:mm) dos agendamentos existentes
        return agendamentos
            .Select(ag => Convert.ToDateTime(ag.dataConsulta).ToString("HH:mm"))
            .Distinct()
            .ToList();
    }
    
    private async Task<List<string>> GetHorariosDisponiveisInterno(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioExpediente = new TimeSpan(8, 0, 0);  // 08:00
        var fimExpediente = new TimeSpan(17, 0, 0);    // 17:00
        var duracaoConsulta = TimeSpan.FromHours(1);

        var horarios = new List<string>();
        for (var hora = inicioExpediente; hora < fimExpediente; hora += duracaoConsulta)
            horarios.Add(hora.ToString(@"hh\:mm"));

        // Pega agendamentos existentes para o dia
        var agendamentos = await _service.GetByDate(dataConsulta, cancellationToken);

        foreach (var ag in agendamentos)
        {
            if (!ag.dataConsulta.HasValue)
                continue;

            // Extrai apenas a hora do agendamento
            var horaAgendada = ag.dataConsulta.Value.TimeOfDay;
            var horaStr = horaAgendada.ToString(@"hh\:mm");

            // Remove horário já ocupado
            horarios.Remove(horaStr);
        }

        return horarios;
    }

    /// <summary>
    /// Retorna todos os agendamentos de um cliente.
    /// </summary>
    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> GetByCliente(string clienteId, CancellationToken cancellationToken)
    {
        var lista = await _service.GetByCliente(clienteId, cancellationToken);
        return Ok(lista);
    }

    /// <summary>
    /// Cria um novo agendamento.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Agendamento agendamento, CancellationToken cancellationToken)
    {
        if (agendamento == null)
            return BadRequest("O corpo da requisição não pode ser nulo.");

        if (agendamento.dataConsulta == null)
            return BadRequest("O campo 'dataConsulta' é obrigatório.");

        var novoAgendamento = new Agendamento
        {
            id = Guid.NewGuid().ToString(),
            animalId = agendamento.animalId,
            clienteId = agendamento.clienteId,
            veterinarioId = agendamento.veterinarioId,
            dataConsulta = agendamento.dataConsulta,
            status = Status.Agendado
        };

        var criado = await _service.Create(novoAgendamento, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.id }, criado);
    }

    /// <summary>
    /// Atualiza o status do agendamento (Agendado, Concluído, Cancelado).
    /// Se o status for "Concluído", cria automaticamente um registro de atendimento.
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromQuery] Status status, CancellationToken cancellationToken)
    {
        var atualizado = await _service.UpdateStatus(id, status, cancellationToken);
        if (atualizado == null)
            return NotFound(new { message = "Agendamento não encontrado para atualização." });

        // Integração automática: cria um atendimento ao concluir a consulta
        if (object.Equals(Status.Concluído, StringComparison.OrdinalIgnoreCase))
        {
            await _atendimentoService.Create(new Atendimento
            {
                animalId = atualizado.animalId,
                clienteId = atualizado.clienteId,
                veterinarioId = atualizado.veterinarioId,
                descricao = "Consulta concluída a partir de agendamento.",
                dataAtendimento = DateTime.UtcNow
            }, cancellationToken);
        }

        return Ok(atualizado);
    }

    /// <summary>
    /// Remove um agendamento.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var ok = await _service.Delete(id, cancellationToken);
        if (!ok)
            return NotFound(new { message = "Agendamento não encontrado para exclusão." });

        return NoContent();
    }
}