using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResponsavelController  : ControllerBase
{
    private IResponsavel _responsavel;
    private readonly UserManager<Responsavel> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IResponsavelService _service;
    public ResponsavelController(UserManager<Responsavel> userManager,IResponsavelService _Service)
    {
        _userManager  = userManager;
        _service = _Service;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Responsavel model)
    {
        var register = await _service.InsetObject(model, CancellationToken.None) as Responsavel;
        return Ok(register);
    }
    [HttpGet("animal")]
    public async Task<IActionResult> Responsavel(string cpf)
    {
        var model = await _service.GetObject(cpf, CancellationToken.None) as Responsavel;
        return Ok(model);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new LoginResult { Success = false, Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
        }

        var user = await _service.FindByEmailAsync(model.credencial, CancellationToken.None) as Responsavel;
        if (user == null || (bool)await _userManager.CheckPasswordAsync(user, model.password))
        {
            return Unauthorized(new LoginResult { Success = false, Message = "Credenciais inválidas." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "120")); // Define o tempo de expiração

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        var login = new LoginResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Message = "Login bem-sucedido!"
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(login.Token);
        var claimsIdentity = new ClaimsIdentity(jwtToken.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
        claimsIdentity.AddClaim(new Claim(user.Id, login.Token));
        return Ok(login);
        
        return null;
    }
}