using System;
using System.Collections.Generic;

namespace ecomServer.Models;

public partial class Size
{
    public string? SizeName { get; set; }

    public bool? IsActive { get; set; }

    public int SizeId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
