using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResponsavelController  : ControllerBase
{
    private readonly IResponsavelService _service;
    public ResponsavelController(IResponsavelService _Service)
    {
        _service = _Service;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Responsavel model)
    {
        var register = await _service.InsetObject(model, CancellationToken.None) as Responsavel;
        return Ok(register);
    }
    [HttpGet("animal")]
    public async Task<IActionResult> Responsavel(string cpf)
    {
        var model = await _service.GetObject(cpf, CancellationToken.None) as Responsavel;
        return Ok(model);
    }
}