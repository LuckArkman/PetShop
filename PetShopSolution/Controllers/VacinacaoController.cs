using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

public record VacinaRegister(string AnimalId, string vacinaId);
public class VacinacaoController : ControllerBase
{
    readonly IVacinacaoService _service;
    readonly IHistoryVacinacaoService _historyVacinacao;
    private readonly IConfiguration _cfg;
    public VacinacaoController(IVacinacaoService service,
        IHistoryVacinacaoService historyVacinacao,
        IConfiguration configuration)
    {
        _service = service;
        _historyVacinacao = historyVacinacao;
        _cfg = configuration;
        _service.InitializeCollection(null, null, "Vacinacao");
        _historyVacinacao.InitializeCollection(null, null, "HistoryVacinacao");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Vacinacao model)
    {
        var register = await _historyVacinacao.GetHistoricoAnimal(model.AnimalId, CancellationToken.None) as HistoryVacinacao;
        if (register is not null)
        {
            register._Vacinacao.Add(model);
            var history = await _historyVacinacao.UpdateObject(register, CancellationToken.None) as HistoryVacinacao;
            if (history is not null) return Ok(model);
        }
        if (register is null)
        {
            var history = new HistoryVacinacao(Guid.NewGuid().ToString(), model.AnimalId, model);
            var _history = await _historyVacinacao.InsetObject(history, CancellationToken.None) as HistoryVacinacao;
            if (_history is not null) return Ok(model);
        }

        return BadRequest(model);
    }

    [HttpGet("historico")]
    public async Task<IActionResult> Historico(string AnimalId)
    {
        var register = await _historyVacinacao.GetHistoricoAnimal(AnimalId, CancellationToken.None) as HistoryVacinacao;
        if (register != null) return Ok(register._Vacinacao);
        return BadRequest();
    }

    [HttpPost("remove")]
    public async Task<IActionResult> remove([FromBody] VacinaRegister model)
    {
        var register = await _historyVacinacao.GetHistoricoAnimal(model.AnimalId, CancellationToken.None) as HistoryVacinacao;
        if (register is not null)
        {
            var vacina = register._Vacinacao.FirstOrDefault(v => v.id == model.vacinaId);
            if (vacina is not null)
            {
                register._Vacinacao.Remove(vacina);
                var history = await _historyVacinacao.UpdateObject(register, CancellationToken.None) as HistoryVacinacao;
                if (history is not null) return Ok(model);
            }
        }

        return BadRequest(model);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Vacinacao model)
    {
        var register = await _service.UpdateObject(model, CancellationToken.None) as VacinaRegister;
        return Ok(register);
    }
}