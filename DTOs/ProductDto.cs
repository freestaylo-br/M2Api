namespace M2Api.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }

    public string Article { get; set; }

    public string ProductName { get; set; }

    public string Category { get; set; }

    public string Manufacturer { get; set; }

    public string Supplier { get; set; }

    public string Measurement { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public int Count { get; set; }

    public string Description { get; set; }

    public string? Photo { get; set; }
}