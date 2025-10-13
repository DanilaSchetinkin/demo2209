using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace demo2209.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int? CodeEmployee { get; set; }

    public string? Fio { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public DateTime? LastEnter { get; set; }

    public string? TypeEnter { get; set; }

    public int? Position { get; set; }

    public string? Imagepath { get; set; }

    public Bitmap Image => new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Imagepath);

    public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? PositionNavigation { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
