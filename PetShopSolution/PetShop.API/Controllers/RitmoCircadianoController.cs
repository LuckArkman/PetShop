using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RitmoCircadianoController : ControllerBase
{
    private readonly IAnimalService _animalService;
    public RitmoCircadianoController(IAnimalService animalService)
    {
        _animalService = animalService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Animal model)
    {
        var register = await _animalService.InsetObject(model, CancellationToken.None) as Animal;
        return Ok(register);
    }
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _animalService.GetObject(animal, CancellationToken.None) as Animal;
        return Ok(model);
    }
}