using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RitmoCircadianoController : ControllerBase
{
    private readonly IRitmoCircadianoService _animalService;
    public RitmoCircadianoController(IRitmoCircadianoService animalService)
    {
        _animalService = animalService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RitmoCircadiano model)
    {
        var register = await _animalService.InsetObject(model, CancellationToken.None) as RitmoCircadiano;
        return Ok(register);
    }
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _animalService.GetObject(animal, CancellationToken.None) as RitmoCircadiano;
        return Ok(model);
    }
}