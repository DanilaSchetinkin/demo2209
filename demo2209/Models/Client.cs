using System;
using System.Collections.Generic;

namespace demo2209.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Fio { get; set; }

    public long? CodeCliend { get; set; }

    public string? Passport { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
