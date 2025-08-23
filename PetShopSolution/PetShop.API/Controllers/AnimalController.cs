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
    private readonly IAnimalService _animalService;  // Campo privado para armazenar o serviço injetado

    // Construtor para injeção de dependência
    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;  // Atribui o serviço injetado ao campo privado
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Animal model)
    {

        return Ok(model);
    }
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = new Animal();
        return Ok(model);
    }
}