using Microsoft.AspNetCore.Mvc;
using PetShop.Application.DTOs;

namespace PetShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalGeolocationController : ControllerBase
{
    public AnimalGeolocationController()
    {
        
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] GeolocationRecord model)
    {

        return Ok(model);
    }
}