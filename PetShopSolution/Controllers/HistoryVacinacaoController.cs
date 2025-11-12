using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoryVacinacaoController  : ControllerBase
{
    readonly IHistoryVacinacaoService _service;
    public HistoryVacinacaoController(IHistoryVacinacaoService service)
    {
        _service = service;
    }
    [HttpGet("HistoryVacinacao")]
    public async Task<IActionResult> HistoryVacinacao(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as HistoryVacinacao;
        return Ok(model);
    }
}