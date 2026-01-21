using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RitmoCircadianoController : ControllerBase
{
    private readonly IRitmoCircadianoService _animalService;
    private readonly IConfiguration _cfg;
    public RitmoCircadianoController(IRitmoCircadianoService animalService,
        IConfiguration configuration)
    {
        _animalService = animalService;
        _cfg = configuration;
        _animalService.InitializeCollection(null, null, "RitmoCircadiano");
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