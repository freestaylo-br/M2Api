namespace M2Api.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public string Article { get; set; } = "";

    public DateTime OrderDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    public int StatusId { get; set; }

    public string StatusName { get; set; } = "";

    public int LocationId { get; set; }

    public string PickupLocation { get; set; } = "";
}