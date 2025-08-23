using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QrCodeRegistroController  : ControllerBase
{
    private readonly IQrCodeRegistroService _service;
    public QrCodeRegistroController(IQrCodeRegistroService service)
    {
        _service = service;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] QrCodeRegistro model)
    {
        var register = await _service.InsetObject(model, CancellationToken.None) as QrCodeRegistro;
        return Ok(register);
    }
    [HttpGet("QrCode")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as QrCodeRegistro;
        return Ok(model);
    }
}