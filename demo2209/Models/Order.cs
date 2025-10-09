using System;
using System.Collections.Generic;

namespace demo2209.Models;

public partial class Order
{
    public int Id { get; set; }

    public string? CodeOrder { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? TimeOrder { get; set; }

    public int? CodeClient { get; set; }

    public string? Status { get; set; }

    public DateTime? DateClose { get; set; }

    public string? TimeRental { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Client? CodeClientNavigation { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Service> Idservices { get; set; } = new List<Service>();
}
