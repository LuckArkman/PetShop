using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalGeolocationController : ControllerBase
{
    readonly IAnimalGeolocationHistoryService  _animalGeolocationHistoryService;
    readonly IGeolocationRecordService _geolocationRecordService;
    public AnimalGeolocationController( IAnimalGeolocationHistoryService animalGeolocationHistoryService,
        IGeolocationRecordService geolocationRecordService)
    {
        _animalGeolocationHistoryService = animalGeolocationHistoryService;
        _animalGeolocationHistoryService =  animalGeolocationHistoryService;
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
    
    [HttpGet("AnimalGeolocation")]
    public async Task<IActionResult> AnimalGeolocation(string cpf)
    {
        var model = await _geolocationRecordService.GetObject(cpf, CancellationToken.None) as GeolocationRecord;
        return Ok(model);
    }
}