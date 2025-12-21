using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoryFrequenciaCardiacaController   : ControllerBase
{
    public IHistoryFrequenciaCardiaca _service { get; set; }
    private readonly IConfiguration _cfg;
    public HistoryFrequenciaCardiacaController(IHistoryFrequenciaCardiaca service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "FrequenciaCardiaca");
    }
    
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as HistoryFrequenciaCardiaca;
        return Ok(model);
    }
}