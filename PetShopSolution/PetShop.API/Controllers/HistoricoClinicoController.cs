using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoricoClinicoController  : ControllerBase
{
    readonly IHistoricoClinicoService _service;
    public HistoricoClinicoController(IHistoricoClinicoService service)
    {
        _service = service;
    }
    /*
    [HttpGet("HistoricoClinico")]
    public async Task<IActionResult> HistoricoClinico(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as HistoricoClinico;
        return Ok(model);
    }
    */
}