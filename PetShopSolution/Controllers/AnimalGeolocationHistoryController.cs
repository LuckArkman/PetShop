using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalGeolocationHistoryController  : ControllerBase
{
    private readonly IAnimalGeolocationHistoryService _service;
    public AnimalGeolocationHistoryController( IAnimalGeolocationHistoryService service)
    {
        _service = service;
    }
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as AnimalGeolocationHistory;
        return Ok(model);
    }
}