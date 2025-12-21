using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioClinicoController : ControllerBase
{
    readonly IHistoricoClinicoService _service;
    readonly IRelatorioClinicoService  _relatorioClinicoService;
    private readonly IMedicoVeterinarioService _medicoVeterinario;
    readonly IConfiguration _cfg;
    public RelatorioClinicoController(
        IHistoricoClinicoService service,
        IRelatorioClinicoService relatorioClinicoService,
        IMedicoVeterinarioService medicoVeterinario,
        IConfiguration cfg)
    {
        _service = service;
        _relatorioClinicoService = relatorioClinicoService;
        _medicoVeterinario = medicoVeterinario;
        _cfg = cfg;
        
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "HistoricoClinico");
        _relatorioClinicoService.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "RelatorioClinico");
        _medicoVeterinario.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "MedicoVeterinario");
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
    
    [HttpGet("Relatorios_Veterinario/{crmv}")]
    public async Task<IActionResult> RelatoriosVeterinario(string crmv)
    {
        var vet = await _medicoVeterinario.FindByCRMVAsync(crmv, CancellationToken.None);
        if (vet == null) return BadRequest();
        var register = await _relatorioClinicoService.GetAllVeterinarioRelatorios(vet.Id,CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("Relatorio")]
    public async Task<IActionResult> Relatorio(string id)
    {
        var model = await _relatorioClinicoService.GetRelatorio(id, CancellationToken.None) as Relatorio;
        return Ok(model);
    }
}