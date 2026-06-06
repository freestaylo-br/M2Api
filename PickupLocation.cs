using System;
using System.Collections.Generic;

namespace M2Api;

public partial class PickupLocation
{
    public int LocationId { get; set; }

    public int Index { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Home { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
