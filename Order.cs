using System;
using System.Collections.Generic;

namespace M2Api;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int ClientId { get; set; }

    public int Code { get; set; }

    public int StatusId { get; set; }

    public int LocationId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Client Client { get; set; } = null!;

    public virtual PickupLocation Location { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
