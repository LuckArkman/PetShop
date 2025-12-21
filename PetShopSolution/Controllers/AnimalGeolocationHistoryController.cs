using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalGeolocationHistoryController  : ControllerBase
{
    private readonly IAnimalGeolocationHistoryService _service;
    private readonly IConfiguration _cfg;
    public AnimalGeolocationHistoryController(IAnimalGeolocationHistoryService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "AnimalGeolocationHistory");
    }
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as AnimalGeolocationHistory;
        return Ok(model);
    }
}