using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DTOs;
using Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AtendenteController : ControllerBase
{
    readonly IAtendenteService _service;
    private readonly IConfiguration _configuration;
    public AtendenteController(IAtendenteService atendenteService,
        IConfiguration configuration)
    {
        _service = atendenteService;
        _configuration = configuration;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Atendente model)
    {
        model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        model.ConfirmPassword = model.Password;
        var check = await _service.FindByEmailAsync(model.email!, CancellationToken.None) as Atendente;
        if (check is not null) return BadRequest(new
        {
            Success = false,
            Token = "",
            Message = "Atendente ja cadastrado!"
        });

        var result = await _service.InsetObject(model, CancellationToken.None) as Atendente;
        return Ok(new { Message = "Atendente registrado com sucesso!", User = result });
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Atendente model)
    {

        var result = await _service.UpdateObject(model, CancellationToken.None) as Atendente;
        Console.WriteLine($"{result == null}");
        return Ok(new { Message = "Atendente atualizado com sucesso!", User = result });
    }
    
    [HttpGet("Atendente{_rg}")]
    public async Task<IActionResult> Atendente(string _rg)
    {

        var result = await _service.GetAtendenteRG(_rg, CancellationToken.None) as Atendente;
        return Ok(result);
    }
    
    [HttpGet("Atendentes")]
    public async Task<IActionResult> Atendentes()
    {

        var result = await _service.GetAllAtendente(CancellationToken.None);
        return Ok(result);
    }

    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string mail)
    {
        var register = await _service.FindByEmailAsync(mail, CancellationToken.None) as Atendente;
        if (register == null) return BadRequest();
        var rm = await _service.RemoveAsync(register.email, CancellationToken.None);
        return Ok(register);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _service.FindByEmailAsync(model.credencial, CancellationToken.None) as Atendente;

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.password, user.Password))
        {
            return BadRequest(new { Success = false, Message = "Credenciais inválidas." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.nome),
            new Claim(ClaimTypes.Email, user.email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "120"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return Ok(new
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Message = "Login bem-sucedido!"
        });
    }
    
}