using System;
using System.Collections.Generic;

namespace EventMVC3.Models;

public partial class CardsPay
{
    public long Id { get; set; }

    public int NumberOfCard { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string Name { get; set; } = null!;

    public short Cvv { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
