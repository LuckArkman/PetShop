using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

public class VacinacaoController : ControllerBase
{
    readonly IVacinacaoService _service;
    readonly IHistoryVacinacaoService _historyVacinacao;
    public VacinacaoController( IVacinacaoService service,
        IHistoryVacinacaoService historyVacinacao)
    {
        _service = service;
        _historyVacinacao = historyVacinacao;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Vacinacao model)
    {
        var register = await _historyVacinacao.GetObject(model.AnimalId, CancellationToken.None) as HistoryVacinacao;
        if (register is not null)
        {
            register._Vacinacao.Add(model);
            var history = await _historyVacinacao.UpdateObject(register, CancellationToken.None) as HistoryVacinacao;
            if (history is not null)return Ok(model);
        }
        if (register is null)
        {
            var history = new HistoryVacinacao(Guid.NewGuid().ToString(), model.AnimalId, model);
            var _history = await _historyVacinacao.InsetObject(history, CancellationToken.None) as HistoryVacinacao;
            if (_history is not null)return Ok(model);
        }
        
        return BadRequest(model);
    }
}