using System;
using System.Collections.Generic;

namespace demo2209.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Employee? Employee { get; set; }
}
