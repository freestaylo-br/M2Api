using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufacturerController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public ManufacturerController(
        KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> Get()
    {
        var manufacturers =
            await _context.Manufacturers
                .Select(x => new
                {
                    ManufacturerId =
                        x.ManufacturerId,

                    ManufacturerName =
                        x.ManufacturerName
                })
                .ToListAsync();

        return Ok(manufacturers);
    }
}