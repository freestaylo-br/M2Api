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
                    x.OrderDate,

                DeliveryDate =
                    x.DeliveryDate
            })

            .ToListAsync();

        return Ok(orders);
    }
}