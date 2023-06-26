using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class OperationsHistory
{
    public DateTime DateTimePurchase { get; set; }

    public int BraceletId { get; set; }

    public int ServiceName { get; set; }

    public int ServiceCount { get; set; }

    public int DiscountPercentage { get; set; }

    public virtual Bracelet Bracelet { get; set; } = null!;

    public virtual Benefit DiscountPercentageNavigation { get; set; } = null!;

    public virtual Service ServiceNameNavigation { get; set; } = null!;
}
