using System;
using System.Collections.Generic;

namespace EventMVC3.Models;

public partial class Comment
{
    public long Id { get; set; }

    public string Comment1 { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
