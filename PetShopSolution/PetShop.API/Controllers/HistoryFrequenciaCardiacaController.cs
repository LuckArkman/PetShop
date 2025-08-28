using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoryFrequenciaCardiacaController   : ControllerBase
{
    public IHistoryFrequenciaCardiaca _service { get; set; }
    public HistoryFrequenciaCardiacaController(IHistoryFrequenciaCardiaca service)
    {
        _service = service;
    }
    [HttpGet("animal")]
    public async Task<IActionResult> animal(string animal)
    {
        var model = await _service.GetObject(animal, CancellationToken.None) as HistoryFrequenciaCardiaca;
        return Ok(model);
    }
}