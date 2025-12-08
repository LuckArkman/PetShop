using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CirurgiaControllers : ControllerBase
{
    public ICirurgiaService _service { get; set; }

    public CirurgiaControllers(ICirurgiaService cirurgiaService)
    {
        _service = cirurgiaService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Cirurgia model)
    {
        var register = await _service.InsetCirurgia(model, CancellationToken.None) as Cirurgia;
        return Ok(register);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Cirurgia model)
    {
        var register = await _service.UpdateCirurgia(model, CancellationToken.None) as Cirurgia;
        return Ok(register);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var register = await _service.GetCirurgia(id, CancellationToken.None) as Cirurgia;
        if (register == null) return BadRequest();
        await _service.RemoveCirurgia(register.Id, CancellationToken.None);
        return Ok(register);
    }
    
    [HttpGet("Cirurgias")]
    public async Task<IActionResult> Cirurgias(string animalId)
    {
        var register = await _service.GetAllCirurgias(animalId,CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("Diagnostico")]
    public async Task<IActionResult> Diagnostico(string id)
    {
        var model = await _service.GetCirurgia(id, CancellationToken.None) as Cirurgia;
        return Ok(model);
    }
}