using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

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
        var register = await _animalService.GetObject(id, CancellationToken.None) as Animal;
        if (register == null) return BadRequest();
        await _animalService.RemoveObject(register, CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _animalService.GetObject(animal, CancellationToken.None) as Animal;
        return Ok(model);
    }
}