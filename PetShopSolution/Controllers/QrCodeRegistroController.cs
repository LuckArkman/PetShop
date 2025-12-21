using Microsoft.AspNetCore.Mvc;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QrCodeRegistroController  : ControllerBase
{
    private readonly IQrCodeRegistroService _service;
    private readonly IConfiguration _cfg;
    public QrCodeRegistroController(IQrCodeRegistroService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "QrCodeRegistro");
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(string AnimalId)
    {
        var model = new QrCodeRegistro
        {
            Id = Guid.NewGuid().ToString(),
            AnimalId = AnimalId,
            DataGeracao = DateTime.UtcNow,
        };
        model.QrCodeBase64 = model.GenerateQrCodeAsBase64String();
        var register = await _service.InsetObject(model, CancellationToken.None) as QrCodeRegistro;
        return Ok(register);
    }
    [HttpGet("QrCode")]
    public async Task<IActionResult> QrCode(string animalId)
    {
        var model = await _service.GetObject(animalId, CancellationToken.None) as QrCodeRegistro;
        return Ok(model);
    }
}