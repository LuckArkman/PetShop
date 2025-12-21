using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoricoClinicoController  : ControllerBase
{
    readonly IHistoricoClinicoService _service;
    readonly IConfiguration _cfg;
    public HistoricoClinicoController(IHistoricoClinicoService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "HistoricoClinico");
    }
    [HttpGet("HistoricoClinico")]
    public async Task<IActionResult> HistoricoClinico(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as HistoricoClinico;
        return Ok(model);
    }
}