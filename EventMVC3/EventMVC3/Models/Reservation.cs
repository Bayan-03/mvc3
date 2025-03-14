using System;
using System.Collections.Generic;

namespace EventMVC3.Models;

public partial class Reservation
{
    public int IdNumber { get; set; }

    public DateTime BookingDate { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
