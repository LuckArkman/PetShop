using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoryVacinacaoController  : ControllerBase
{
    readonly IHistoryVacinacaoService _service;
    private readonly IConfiguration _cfg;
    public HistoryVacinacaoController(IHistoryVacinacaoService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "HistoryVacinacao");
    }
    [HttpGet("HistoryVacinacao")]
    public async Task<IActionResult> HistoryVacinacao(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as HistoryVacinacao;
        return Ok(model);
    }
}