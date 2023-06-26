using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Result
{
    public DateTimeOffset Workday { get; set; } //DateOnly?

    public decimal? TotalAmount { get; set; }
}
