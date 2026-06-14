using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M2Api.DTOs;

namespace M2Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly KarpovSpContext _context;

    public OrdersController(KarpovSpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders =
            await _context.Orders

            .Include(x => x.Status)

            .Include(x => x.Location)

            .Include(x => x.Carts)

            .ThenInclude(x => x.Product)

            .Select(x => new OrderDto
            {
                OrderId = x.OrderId,

                Article =
                    x.Carts
                        .Select(c => c.Product.Article)
                        .FirstOrDefault() ?? "",

                ProductId =
                    x.Carts
                        .Select(c => c.ProductId)
                        .FirstOrDefault(),

                StatusId =
                    x.StatusId,

                StatusName =
                    x.Status.StatusName,

                PickupLocation =
                    x.Location.City + ", " +
                    x.Location.Street + ", " +
                    x.Location.Home,

                LocationId =
                    x.LocationId,

                OrderDate =
                    x.OrderDate.ToDateTime(
                        TimeOnly.MinValue),

                DeliveryDate =
                    x.DeliveryDate.ToDateTime(
                        TimeOnly.MinValue),
            })

            .ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
    [FromBody] OrderDto dto)
    {
        if (dto.OrderId != 0)
        {
            return BadRequest(
                $"OrderId пришел = {dto.OrderId}");
        }

        var client =
            await _context.Clients
                .FirstOrDefaultAsync();

        if (client == null)
            return BadRequest(
                "Не найден клиент");

        var product =
            await _context.Products
                .FirstOrDefaultAsync(
                    x => x.ProductId ==
                         dto.ProductId);

        if (product == null)
            return BadRequest(
                "Товар не найден");

        var order = new Order
        {
            ClientId =
                client.ClientId,

            StatusId =
                dto.StatusId,

            LocationId =
                dto.LocationId,

            OrderDate =
                DateOnly.FromDateTime(
                    dto.OrderDate),

            DeliveryDate =
                DateOnly.FromDateTime(
                    dto.DeliveryDate),

            Code =
                Random.Shared.Next(
                    100,
                    999)
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        var cart = new Cart
        {
            OrderId =
                order.OrderId,

            ProductId =
                dto.ProductId,

            Count = 1
        };

        _context.Carts.Add(cart);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(
    int id,
    [FromBody] OrderDto dto)
    {
        var order =
            await _context.Orders
                .Include(x => x.Carts)
                .FirstOrDefaultAsync(
                    x => x.OrderId == id);

        if (order == null)
            return NotFound();

        order.StatusId = dto.StatusId;
        order.LocationId = dto.LocationId;

        order.OrderDate =
            DateOnly.FromDateTime(dto.OrderDate);

        order.DeliveryDate =
            DateOnly.FromDateTime(dto.DeliveryDate);

        var cart =
            order.Carts.FirstOrDefault();

        if (cart != null)
        {
            cart.ProductId = dto.ProductId;
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(
    int id)
    {
        var order =
            await _context.Orders
                .Include(x => x.Carts)
                .FirstOrDefaultAsync(
                    x => x.OrderId == id);

        if (order == null)
            return NotFound();

        _context.Carts.RemoveRange(
            order.Carts);

        _context.Orders.Remove(
            order);

        await _context.SaveChangesAsync();

        return Ok();
    }
}