using Microsoft.AspNetCore.Mvc;
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
    
    // ===========================================================
    // 🔹 ROTA 1: Retorna dias disponíveis no calendário
    // ===========================================================
    [HttpGet("disponiveis")]
    public async Task<IActionResult> GetDiasDisponiveis(CancellationToken cancellationToken)
    {
        var indisponiveis = await _disponibilidadeService.GetIndisponiveis(cancellationToken);
        var diasIndisponiveis = indisponiveis.Select(d => d.Data.Date).ToHashSet();

        var hoje = DateTime.Today;
        var dias = new List<string>();

        // Exibir os próximos 30 dias
        for (int i = 0; i < 30; i++)
        {
            var dia = hoje.AddDays(i);
            // Ignora domingos
            if (dia.DayOfWeek == DayOfWeek.Sunday)
                continue;

            // Se o dia estiver marcado como indisponível, pula
            if (diasIndisponiveis.Contains(dia.Date))
                continue;

            dias.Add(dia.ToString("yyyy-MM-dd"));
        }

        return Ok(dias);
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
        var criado = await _service.Create(agendamento, cancellationToken);
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