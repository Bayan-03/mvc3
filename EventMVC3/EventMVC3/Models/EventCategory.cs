using System;
using System.Collections.Generic;

namespace EventMVC3.Models;

public partial class EventCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
