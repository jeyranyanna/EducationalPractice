using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Result
{
    public DateOnly? Workday { get; set; }

    public decimal? TotalAmount { get; set; }
}
