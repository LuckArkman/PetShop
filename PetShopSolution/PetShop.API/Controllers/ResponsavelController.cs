using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class ResponsavelController : ControllerBase
{
    private readonly IResponsavelService _service;
    private readonly IConfiguration _configuration;
    private readonly IAnimalService _animalService;

    public ResponsavelController(IResponsavelService service,
        IAnimalService animalService,
        IConfiguration configuration)
    {
        _service = service;
        _animalService = animalService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Responsavel model)
    {
        model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        model.ConfirmPassword = model.Password;
        var check = await _service.FindByEmailAsync(model.Email, CancellationToken.None) as Responsavel;
        if (check is not null) return BadRequest(new
        {
            Success = false,
            Token = "",
            Message = "Usuario ja cadastrado!"
        });

        var result = await _service.InsetObject(model, CancellationToken.None) as Responsavel;
        return Ok(new { Message = "Usuário registrado com sucesso!", User = result });
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Responsavel model)
    {

        var result = await _service.UpdateObject(model, CancellationToken.None) as Responsavel;
        return Ok(new { Message = "Usuário atualizado com sucesso!", User = result });
    }
    
    [HttpGet("Responsaveis")]
    public async Task<IActionResult> Responsaveis()
    {

        var result = await _service.GetAllResponsavel(CancellationToken.None);
        return Ok(result);
    }
    
    [HttpGet("animais")]
    public async Task<IActionResult> Animais(string id)
    {
        var register = await _service.GetObject(id, CancellationToken.None) as Responsavel;
        var animals = register.Animais;
        if (animals is not null)
        {
            var animalsInList = _animalService.GetAnimalsInList(animals, CancellationToken.None);
            return Ok(animalsInList);
        }
        return Ok(animals);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string mail)
    {
        var register = await _service.GetObject(mail, CancellationToken.None) as Responsavel;
        if (register == null) return BadRequest();
        var rm = await _service.RemoveAsync(register.Email, CancellationToken.None);
        return Ok(register);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _service.FindByEmailAsync(model.credencial, CancellationToken.None) as Responsavel;

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.password, user.Password))
        {
            return Unauthorized(new { Success = false, Message = "Credenciais inválidas." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email)
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
