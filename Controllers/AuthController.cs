using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M2Api.DTOs;
using M2Api;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public AuthController(KarpovSpContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user = await _context.Staff
            .FirstOrDefaultAsync(x =>
                x.Login == request.Login &&
                x.Password == request.Password);

        if (user == null)
        {
            return Unauthorized("Неверный логин или пароль");
        }

        return Ok(user);
    }
}