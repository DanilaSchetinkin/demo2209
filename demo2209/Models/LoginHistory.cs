using System;
using System.Collections.Generic;

namespace demo2209.Models;

public partial class LoginHistory
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? LoginTime { get; set; }

    public string? LoginType { get; set; }

    public virtual Employee? Employee { get; set; }
}
