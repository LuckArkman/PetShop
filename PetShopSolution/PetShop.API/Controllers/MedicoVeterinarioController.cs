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
public class MedicoVeterinarioController  : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<MedicoVeterinario> _userManager;
    private readonly IConfiguration _configuration;
    readonly IMedicoVeterinarioService _service;
    public MedicoVeterinarioController(UserManager<MedicoVeterinario> userManager,IMedicoVeterinarioService service)
    {
        _userManager = userManager;
        _service = service;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] MedicoVeterinario model)
    {
        var register = await _service.InsetObject(model, CancellationToken.None) as MedicoVeterinario;
        return Ok(register);
    }
    [HttpGet("MedicoVeterinario")]
    public async Task<IActionResult> MedicoVeterinario(string crmv)
    {
        var model = await _service.GetObject(crmv, CancellationToken.None) as MedicoVeterinario;
        return Ok(model);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new LoginResult { Success = false, Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
        }

        var user = await _service.FindByCRMVAsync(model.credencial, CancellationToken.None) as MedicoVeterinario;
        if (user == null || (bool)await _userManager.CheckPasswordAsync(user, model.password))
        {
            return Unauthorized(new LoginResult { Success = false, Message = "Credenciais inválidas." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Nome),
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