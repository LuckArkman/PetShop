using Microsoft.AspNetCore.Mvc;
using DTOs;
using Enums;
using Interfaces;

namespace PetShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaixaController  : ControllerBase
{
    readonly ICaixaService _caixaService;
    
    public CaixaController(ICaixaService caixaService)
    {
        _caixaService = caixaService;
    }
    [HttpGet("GetByIdAsync/{id}")]
    public async Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var pg = await _caixaService.GetById(id, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);

    }
    [HttpGet("GetPagamentosCompletosDoDiaAsync/{dataConsulta}")]
    public async Task<IActionResult> GetPagamentosCompletosDoDiaAsync(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var hoje = dataConsulta.Date;
        var pg = await _caixaService.GetAllTodayPaidsCompletes(hoje, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpGet("GetPagamentosPendentesDoDiaAsync/{dataConsulta}")]
    public async Task<IActionResult> GetPagamentosPendentesDoDiaAsync(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var hoje = dataConsulta.Date;
        var pg = await _caixaService.GetAllTodayPaidsPending(hoje, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpGet("GetPagamentosCanceladosDoDiaAsync/{dataConsulta}")]
    public async Task<IActionResult> GetPagamentosCanceladosDoDiaAsync(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var hoje = dataConsulta.Date;
        var pg = await _caixaService.GetAllTodayPaidsCanceled(hoje, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpGet("GetByClienteCpfAsync/{cpf}")]
    public async Task<IActionResult> GetByClienteCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        var pg = await _caixaService.GetByCliente(cpf, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpPost("CreateAsync")]
    public async Task<IActionResult> CreateAsync(Pagamento pagamento, CancellationToken cancellationToken)
    {
        var pg = await _caixaService.Create(pagamento, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpPost("UpdateStatusAsync")]
    public async Task<IActionResult> UpdateStatusAsync(string id, PaidStatus status, CancellationToken cancellationToken)
    {
        var pg = await _caixaService.UpdateStatus(id, status, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpDelete("DeleteByDataHoraAsync/{dataHora}")]
    public async Task<IActionResult> DeleteByDataHoraAsync(DateTime dataHora, CancellationToken cancellationToken)
    {
        var hoje = dataHora.Date;
        var pg = await _caixaService.DeleteByDateTime(hoje, cancellationToken);
        if (pg) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpDelete("DeleteAsync/{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var pg = await _caixaService.Delete(id, cancellationToken);
        if (pg) return new NotFoundResult();
        return Ok(pg);
    }
}