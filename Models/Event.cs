using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public DateOnly DateTime { get; set; }

    public TimeOnly Duration { get; set; }

    public string? Description { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Staff? Employee { get; set; }
}
