using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Vedomost
{
    public DateTime? DateTimePurchase { get; set; }

    public int? BraceletId { get; set; }

    public string? ServiceName { get; set; }

    public decimal? Price { get; set; }

    public int? ServiceCount { get; set; }

    public int? DiscountPercentage { get; set; }
}
