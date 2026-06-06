using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M2Api.DTOs;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public ProductsController(KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Manufacturer)
            .Include(x => x.Supplier)
            .Include(x => x.ProductName)
            .Select(x => new ProductDto
            {
                ProductId = x.ProductId,
                Article = x.Article,

                ProductName = x.ProductName.Name,
                Category = x.Category.CategoryName,
                Manufacturer = x.Manufacturer.ManufacturerName,
                Supplier = x.Supplier.SupplierName,

                Measurement = x.Measurement,
                Amount = x.Amount,
                Discount = x.Discount,
                Count = x.Count,
                Description = x.Description,
                Photo = x.Photo,
            })
            .ToListAsync();

        return Ok(products);
    }
}