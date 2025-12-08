using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FrequenciaCardiacaController  : ControllerBase
{
    public IHistoryFrequenciaCardiaca _service { get; set; }
    public IFrequenciaCardiaca _frequenciaCardiaca { get; set; }
    public FrequenciaCardiacaController(IHistoryFrequenciaCardiaca service,  IFrequenciaCardiaca frequenciaCardiaca)
    {
        _service = service;
        _frequenciaCardiaca = frequenciaCardiaca;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] FrequenciaCardiaca model)
    {
        var register = await _service.GetObject(model.AnimalId, CancellationToken.None) as HistoryFrequenciaCardiaca;
        if (register is not null)
        {
            register._frequenciaCardiaca.Add(model);
            var update = await _service.UpdateObject(register, CancellationToken.None);
            if (update is not null)
            {
                var frequence = await _frequenciaCardiaca.UpdateObject(model, CancellationToken.None);
                return Ok(model);
            }
        }
        if (register is null)
        {
            register = new HistoryFrequenciaCardiaca(Guid.NewGuid().ToString(),model.AnimalId, model);
            var update = await _service.UpdateObject(register, CancellationToken.None) as  HistoryFrequenciaCardiaca;
            if (update is not null)
            {
                var frequence = await _frequenciaCardiaca.InsetObject(model, CancellationToken.None);
                return Ok(model);
            }
        }

        return BadRequest(model);
    }
}