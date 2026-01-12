using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalGeolocationController : ControllerBase
{
    readonly IAnimalGeolocationHistoryService  _animalGeolocationHistoryService;
    readonly IGeolocationRecordService _geolocationRecordService;
    private readonly IConfiguration _cfg;
    public AnimalGeolocationController(
        IConfiguration cfg,
        IAnimalGeolocationHistoryService animalGeolocationHistoryService,
        IGeolocationRecordService geolocationRecordService)
    {
        _cfg = cfg;
        _animalGeolocationHistoryService = animalGeolocationHistoryService;
        _animalGeolocationHistoryService.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "AnimalGeolocationHistory");
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] GeolocationRecord model)
    {
        var _obj = await _animalGeolocationHistoryService.GetObject(model.AnimalId, CancellationToken.None) as AnimalGeolocationHistory;
        if (_obj is not null)
        {
            _obj.Locations.Add(model);
            var history = await _animalGeolocationHistoryService.UpdateObject(_obj, CancellationToken.None) as AnimalGeolocationHistory;
            if (history is not null)return Ok(model);
        }
        if (_obj is  null)
        {
            _obj = new AnimalGeolocationHistory(model.AnimalId, model);
            _obj.Locations.Add(model);
            var history = await _animalGeolocationHistoryService.UpdateObject(_obj, CancellationToken.None) as AnimalGeolocationHistory;
            if (history is not null)return Ok(model);
        }
        return BadRequest(model);
    }
    
    [HttpGet("AnimalGeolocation/{Id}")]
    public async Task<IActionResult> AnimalGeolocation(string Id)
    {
        var model = await _animalGeolocationHistoryService.GetObject(Id, CancellationToken.None) as AnimalGeolocationHistory;
        return Ok(model);
    }
}