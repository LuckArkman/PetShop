using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicoVeterinarioController  : ControllerBase
{
    readonly IMedicoVeterinarioService _service;
    public MedicoVeterinarioController(IMedicoVeterinarioService service)
    {
        _service = service;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] MedicoVeterinario model)
    {
        var register = await _service.InsetObject(model, CancellationToken.None) as MedicoVeterinario;
        return Ok(register);
    }
    [HttpGet("MedicoVeterinario")]
    public async Task<IActionResult> MedicoVeterinario(string crmv)
    {
        var model = await _service.GetObject(crmv, CancellationToken.None) as MedicoVeterinario;
        return Ok(model);
    }
}