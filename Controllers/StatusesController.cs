using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusesController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public StatusesController(KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatuses()
    {
        return Ok(await _context.Statuses.ToListAsync());
    }
}