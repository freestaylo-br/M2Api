using System;
using System.Collections.Generic;

namespace M2Api;

public partial class ProductName
{
    public int ProductNameId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}