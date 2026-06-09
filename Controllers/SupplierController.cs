using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public SuppliersController(
        KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> Get()
    {
        var suppliers =
            await _context.Suppliers
                .Select(x => new
                {
                    SupplierId =
                        x.SupplierId,

                    supplier_name =
                        x.supplier_name
                })
                .ToListAsync();

        return Ok(suppliers);
    }
}