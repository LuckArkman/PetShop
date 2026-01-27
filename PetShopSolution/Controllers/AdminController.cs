using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace PetShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController  : ControllerBase
{
    private readonly IAdminService _service;
    private readonly IConfiguration _cfg;
    public AdminController(IAdminService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
        _service.InitializeCollection(null, null, "Admin");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        model.Senha = GerarHashSenha(model.Senha);
        model.ConfirmarSenha = model.Senha;
        var check = await _service.FindByEmailAsync(model.Email, CancellationToken.None) as RegisterViewModel;
        if (check is not null) return BadRequest(new
        {
            Success = false,
            Token = "",
            Message = "Administrador ja cadastrado!"
        });
        var register = await _service.InsetObject(model, CancellationToken.None) as RegisterViewModel;
        return Ok(register);
    }
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] RegisterViewModel model)
    {
        var register = await _service.UpdateObject(model, CancellationToken.None) as RegisterViewModel;
        return Ok(register);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string mail)
    {
        var register = await _service.GetObject(mail, CancellationToken.None) as RegisterViewModel;
        if (register == null) return BadRequest();
        var rm = await _service.RemoveAsync(register.Email, CancellationToken.None);
        return Ok(register);
    }

    [HttpPost("login")]
public async Task<ActionResult> Login([FromBody] LoginRequest model)
{
    var user = await _service.FindByEmailAsync(model.credencial, CancellationToken.None) as RegisterViewModel;
    
    // Importante: Verifique se sua lógica de hash condiz com o que está no banco
    var passwordHash = GerarHashSenha(model.password);

    if (user == null || !Verify(passwordHash, user.Senha))
    {
        return BadRequest(new { Success = false, Message = "Credenciais inválidas." });
    }

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        new Claim(ClaimTypes.Name, user.Nome),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, "Admin") // Adicionado para autorização
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expiresValue = Convert.ToDouble(_cfg["Jwt:ExpireMinutes"] ?? "120");
    var expires = DateTime.UtcNow.AddMinutes(expiresValue);

    var token = new JwtSecurityToken(
        issuer: _cfg["Jwt:Issuer"],
        audience: _cfg["Jwt:Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    // --- CONFIGURAÇÃO DO COOKIE HTTPONLY ---
    var cookieOptions = new CookieOptions
    {
        HttpOnly = true,        
        Secure = true,          
        SameSite = SameSiteMode.Strict, 
        Expires = expires       
    };

    // Adiciona o cookie na resposta
    Response.Cookies.Append("X-Access-Token", tokenString, cookieOptions);

    return Ok(new
    {
        Success = true,
        Message = "Login bem-sucedido!"
        // Note que o Token NÃO é enviado no JSON
    });
}

[HttpPost("logout")]
public IActionResult Logout()
{
    Response.Cookies.Delete("X-Access-Token");
    return Ok(new { Message = "Logout realizado com sucesso" });
}

    private bool Verify(string modelPassword, string userPassword)
        => modelPassword == userPassword;

    string GerarHashSenha(string email)
    {
        // Combina o e-mail com a senha (você pode usar outro formato se preferir)
        string entrada = $"{email}";

        // Converte para bytes
        byte[] bytesEntrada = Encoding.UTF8.GetBytes(entrada);

        // Cria o hash SHA256
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(bytesEntrada);

            // Converte o hash para string hexadecimal
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString(); // Hash final em hexadecimal
        }
    }
}