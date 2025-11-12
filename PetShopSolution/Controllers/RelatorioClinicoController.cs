using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioClinicoController : ControllerBase
{
    readonly IHistoricoClinicoService _service;
    readonly IRelatorioClinicoService  _relatorioClinicoService;
    public RelatorioClinicoController(IHistoricoClinicoService service,
        IRelatorioClinicoService relatorioClinicoService)
    {
        _service = service;
        _relatorioClinicoService = relatorioClinicoService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Relatorio model)
    {
        var register = await _relatorioClinicoService.InsetRelatorio(model, CancellationToken.None) as Relatorio;
        return Ok(register);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Relatorio model)
    {
        var register = await _relatorioClinicoService.UpdateRelatorio(model, CancellationToken.None) as Relatorio;
        return Ok(register);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var register = await _relatorioClinicoService.GetRelatorio(id, CancellationToken.None) as Relatorio;
        if (register == null) return BadRequest();
        await _relatorioClinicoService.RemoveRelatorio(register, CancellationToken.None);
        return Ok(register);
    }
    
    [HttpGet("Relatorios")]
    public async Task<IActionResult> Relatorios(string animalId)
    {
        var register = await _relatorioClinicoService.GetAllRelatorios(animalId,CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("Relatorio")]
    public async Task<IActionResult> Relatorio(string id)
    {
        var model = await _relatorioClinicoService.GetRelatorio(id, CancellationToken.None) as Relatorio;
        return Ok(model);
    }
}