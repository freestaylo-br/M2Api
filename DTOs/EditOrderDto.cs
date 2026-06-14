namespace M2Api.DTOs;

public class EditOrderDto
{
    public int OrderId { get; set; }

    public string Article { get; set; } = "";

    public int StatusId { get; set; }

    public int LocationId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }
}