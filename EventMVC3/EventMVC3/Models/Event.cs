using System;
using System.Collections.Generic;

namespace EventMVC3.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public TimeSpan StaetTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string? PlaceName { get; set; }

    public string? City { get; set; }

    public string Discription { get; set; } = null!;

    public int? UserId { get; set; }

    public int Category { get; set; }

    public int? Price { get; set; }

    public virtual EventCategory CategoryNavigation { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual User? User { get; set; }
}
