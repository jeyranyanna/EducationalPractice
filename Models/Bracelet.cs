using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Bracelet
{
    public int BraceletId { get; set; }

    public decimal CashBalance { get; set; }

    public TimeOnly RemainingTime { get; set; }

    public int? LockerNumber { get; set; }

    public virtual ICollection<OperationsHistory> OperationsHistories { get; set; } = new List<OperationsHistory>();
}
