using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosticoController : ControllerBase
{
    public IDiagnosticoService _service { get; set; }
    private readonly IConfiguration _cfg;
    public DiagnosticoController(IDiagnosticoService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "Diagnostico");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Diagnostico model)
    {
        var register = await _service.InsetDiagnostico(model, CancellationToken.None) as Diagnostico;
        return Ok(register);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Diagnostico model)
    {
        var register = await _service.UpdateDiagnostico(model, CancellationToken.None) as Diagnostico;
        return Ok(register);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var register = await _service.GetDiagnostico(id, CancellationToken.None) as Diagnostico;
        if (register == null) return BadRequest();
        await _service.RemoveDiagnostico(register.Id, CancellationToken.None);
        return Ok(register);
    }
    
    [HttpGet("Diagnosticos")]
    public async Task<IActionResult> Diagnosticos(string animalId)
    {
        var register = await _service.GetAllRelatorios(animalId,CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("Diagnostico")]
    public async Task<IActionResult> Diagnostico(string id)
    {
        var model = await _service.GetDiagnostico(id, CancellationToken.None) as Diagnostico;
        return Ok(model);
    }
}