using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public CategoriesController(
        KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> Get()
    {
        var categories =
            await _context.Categories
                .Select(x => new
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName
                })
                .ToListAsync();

        return Ok(categories);
    }
}