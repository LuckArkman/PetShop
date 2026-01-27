using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class ResponsavelController : ControllerBase
{
    private readonly IResponsavelService _service;
    private readonly IConfiguration _cfg;
    private readonly IAnimalService _animalService;

    public ResponsavelController(IResponsavelService service,
        IAnimalService animalService,
        IConfiguration configuration)
    {
        _service = service;
        _animalService = animalService;
        _cfg = configuration;
        _service.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "Responsavel");
        _animalService.InitializeCollection(_cfg["MongoDbSettings:ConnectionString"],
            _cfg["MongoDbSettings:DataBaseName"],
            "Animais");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Responsavel model)
    {
        Console.WriteLine($"{nameof(Register)}: {model}");
        model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        model.ConfirmPassword = model.Password;
        var check = await _service.GetObject(model.Email!, CancellationToken.None) as Responsavel;
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
        Console.WriteLine($"{nameof(Update)}: {model}");
        var result = await _service.UpdateAnaimalListObject(model, CancellationToken.None) as Responsavel;
        Console.WriteLine($"{result == null}");
        return Ok(new { Message = "Usuário atualizado com sucesso!", User = result });
    }
    
    [HttpGet("Responsavel{_rg}")]
    public async Task<IActionResult> Responsavel(string _rg)
    {
        Console.WriteLine($"{nameof(Responsavel)}: {_rg}");
        var result = await _service.GetResponsavelId(_rg, CancellationToken.None) as Responsavel;
        return Ok(result);
    }
    
    [HttpGet("Responsaveis")]
    public async Task<IActionResult> Responsaveis()
    {
        Console.WriteLine($"{nameof(Responsaveis)}");
        var result = await _service.GetAllResponsavel(CancellationToken.None);
        return Ok(result);
    }

    [HttpPost("animais")]
    public async Task<IActionResult> Animais([FromBody] Mail mail)
    {
        Console.WriteLine($"{nameof(Animais)}: {mail.mail}");
    
        var register = await _service.GetObject(mail.mail, CancellationToken.None) as Responsavel;

        // 1. Check if the user exists
        if (register == null)
        {
            return NotFound(new { Message = "Responsável não encontrado." });
        }

        // 2. Safely check if the list is null or empty
        if (register.Animais == null || register.Animais.Count == 0)
        {
            return Ok(Array.Empty<object>());
        }

        var animals = await _animalService.GetAnimalsInList(register.Animais, CancellationToken.None);
        return Ok(animals);
    }

    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string mail)
    {
        Console.WriteLine($"{nameof(Delete)}: {mail}");
        var register = await _service.GetObject(mail, CancellationToken.None) as Responsavel;
        if (register == null) return BadRequest();
        var rm = await _service.RemoveAsync(register.Email, CancellationToken.None);
        return Ok(register);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        Console.WriteLine($"{nameof(Login)}: {model}");
        var user = await _service.FindByEmailAsync(model.credencial, CancellationToken.None) as Responsavel;

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.password, user.Password))
        {
            return BadRequest(new { Success = false, Message = "Credenciais inválidas." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_cfg["Jwt:ExpireMinutes"] ?? "120"));

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
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
