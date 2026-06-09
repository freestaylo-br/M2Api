using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
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
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
    string? searchTerm = null,
    bool isSortDescending = false,
    int? supplierId = null)
    {
        var query = _context.Products
            .Include(p => p.ProductName)
            .Include(p => p.Category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var words = searchTerm
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                query = query.Where(p =>
                    p.ProductName.Name.ToLower().Contains(word) ||
                    p.Category.CategoryName.ToLower().Contains(word) ||
                    p.Manufacturer.ManufacturerName.ToLower().Contains(word) ||
                    p.Supplier.supplier_name.ToLower().Contains(word) ||
                    p.Description.ToLower().Contains(word));
            }
        }

        if (supplierId.HasValue)
        {
            query = query.Where(p => p.SupplierId == supplierId.Value);
        }

        query = isSortDescending
            ? query.OrderByDescending(p => p.Count)
            : query.OrderBy(p => p.Count);

        var products = await query
            .Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Article = p.Article,
                ProductName = p.ProductName.Name,
                Category = p.Category.CategoryName,
                Manufacturer = p.Manufacturer.ManufacturerName,
                Supplier = p.Supplier.supplier_name,
                Amount = p.Amount,
                Discount = p.Discount,
                Count = p.Count,
                Description = p.Description,
                Photo = p.Photo,
                Measurement = p.Measurement
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
     [FromForm] string productJson,
     IFormFile? image)
    {
        var dto =
            JsonSerializer.Deserialize<ProductDto>(
                productJson);

        if (dto == null)
            return BadRequest();

        string? fileName = null;

        if (image != null)
        {
            fileName =
                Guid.NewGuid() +
                Path.GetExtension(image.FileName);

            var path =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    fileName);

            using var stream =
                new FileStream(
                    path,
                    FileMode.Create);

            await image.CopyToAsync(stream);
        }

        var productName =
            await _context.ProductNames
                .FirstOrDefaultAsync(x =>
                    x.Name.Trim().ToLower() ==
                    dto.ProductName.Trim().ToLower());

        if (productName == null)
        {
            return BadRequest(
                $"Не найден продукт нэйм: {dto.ProductName}");
        }

        var category =
            await _context.Categories
                .FirstOrDefaultAsync(x =>
                    x.CategoryName == dto.Category);

        var manufacturer =
            await _context.Manufacturers
                .FirstOrDefaultAsync(x =>
                    x.ManufacturerName ==
                    dto.Manufacturer);

        var supplier =
            await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.supplier_name ==
                    dto.Supplier);

        if (category == null ||
            manufacturer == null ||
            supplier == null)
        {
            return BadRequest(
                "Не найдены связанные данные");
        }

        var product = new Product
        {
            Article = dto.Article ?? "",

            ProductNameId =
                productName.ProductNameId,

            Measurement =
                dto.Measurement ?? "",

            Amount =
                dto.Amount,

            SupplierId =
                supplier.SupplierId,

            ManufacturerId =
                manufacturer.ManufacturerId,

            CategoryId =
                category.CategoryId,

            Discount =
                dto.Discount,

            Count =
                dto.Count,

            Description =
                dto.Description,

            Photo =
                fileName
        };

        _context.Products.Add(product);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(
                ex.InnerException?.Message ??
                ex.Message);
        }

        return Ok();
    }
}