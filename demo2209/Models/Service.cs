using System;
using System.Collections.Generic;

namespace demo2209.Models;

public partial class Service
{
    public string? ServiceName { get; set; }

    public string? ServiceCode { get; set; }

    public string? Cost { get; set; }

    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public virtual ICollection<Order> Idorders { get; set; } = new List<Order>();
}
