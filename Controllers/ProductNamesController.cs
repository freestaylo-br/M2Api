using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductNamesController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public ProductNamesController(KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.ProductNames.ToListAsync());
    }
}