using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;
using PetShop.Application.Services;

namespace PetShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtendimentoController : ControllerBase
{
    private readonly AtendimentoService _service;

    public AtendimentoController(AtendimentoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna um atendimento específico pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var atendimento = await _service.GetById(id, cancellationToken);
        if (atendimento == null)
            return NotFound(new { message = "Atendimento não encontrado." });

        return Ok(atendimento);
    }

    /// <summary>
    /// Retorna todos os atendimentos de um animal.
    /// </summary>
    [HttpGet("animal/{animalId}")]
    public async Task<IActionResult> GetByAnimal(string animalId, CancellationToken cancellationToken)
    {
        var lista = await _service.GetByAnimal(animalId, cancellationToken);
        return Ok(lista);
    }

    /// <summary>
    /// Cria um novo atendimento.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Atendimento atendimento, CancellationToken cancellationToken)
    {
        var criado = await _service.Create(atendimento, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.id }, criado);
    }

    /// <summary>
    /// Atualiza um atendimento existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Atendimento atendimento, CancellationToken cancellationToken)
    {
        atendimento.id = id;
        var atualizado = await _service.Update(atendimento, cancellationToken);

        if (atualizado == null)
            return NotFound(new { message = "Atendimento não encontrado para atualização." });

        return Ok(atualizado);
    }

    /// <summary>
    /// Remove um atendimento.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var ok = await _service.Delete(id, cancellationToken);
        if (!ok)
            return NotFound(new { message = "Atendimento não encontrado para exclusão." });

        return NoContent();
    }
}