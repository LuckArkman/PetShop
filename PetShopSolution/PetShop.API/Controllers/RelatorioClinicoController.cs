using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

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
    public async Task<IActionResult> Register([FromBody] RelatorioClinico model)
    {
        var register = await _service.GetObject(model.AnimalId, CancellationToken.None) as HistoricoClinico;
        if (register is not null)
        {
            register.Relatorios.Add(model);
            var update = await _service.UpdateObject(register, CancellationToken.None) as  RelatorioClinico;
            return Ok(model);
        }
        if (register is null)
        {
            register = new HistoricoClinico(model.AnimalId, model);
            register.Relatorios.Add(model);
            var update = await _service.UpdateObject(register, CancellationToken.None) as  RelatorioClinico;
            return Ok(model);
        }

        return BadRequest(model);
    }
}