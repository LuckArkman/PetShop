using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicacaoController  : ControllerBase
{
    readonly IMedicacaoService _medicacaoService;
    public MedicacaoController(IMedicacaoService  medicacaoService)
    {
        _medicacaoService  = medicacaoService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Medicacao model)
    {
        var register = await _medicacaoService.InsetMedicacao(model, CancellationToken.None) as Medicacao;
        return Ok(register);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Medicacao model)
    {
        var register = await _medicacaoService.UpdateMedicacao(model, CancellationToken.None) as Medicacao;
        return Ok(register);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var register = await _medicacaoService.GetMedicacao(id, CancellationToken.None) as Medicacao;
        if (register == null) return BadRequest();
        await _medicacaoService.RemoveMedicacao(register, CancellationToken.None);
        return Ok(register);
    }
    
    [HttpGet("Medicacoes")]
    public async Task<IActionResult> Medicacoes(string animalId)
    {
        var register = await _medicacaoService.GetAllMedicacoes(animalId,CancellationToken.None);
        return Ok(register);
    }

    [HttpGet("Medicacao")]
    public async Task<IActionResult> Medicacao(string id)
    {
        var model = await _medicacaoService.GetMedicacao(id, CancellationToken.None) as Medicacao;
        return Ok(model);
    }
}