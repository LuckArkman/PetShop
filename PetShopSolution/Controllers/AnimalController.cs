using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;
    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Animal model)
    {
        var register = await _animalService.InsetObject(model, CancellationToken.None) as Animal;
        return Ok(register);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Animal model)
    {
        var register = await _animalService.UpdateObject(model, CancellationToken.None) as Animal;
        return Ok(register);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var remove = await _animalService.RemoveObject(id, CancellationToken.None);
        return Ok(remove);
    }
    
    [HttpGet("animais")]
    public async Task<IActionResult> Animais()
    {
        var register = await _animalService.GetAllAnimals(CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("animal/{id}")]
    public async Task<IActionResult> animal(string id)
    {
        var model = await _animalService.GetObject(id, CancellationToken.None) as Animal;
        return Ok(model);
    }
}