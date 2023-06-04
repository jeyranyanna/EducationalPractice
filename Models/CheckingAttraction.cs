using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class CheckingAttraction
{
    public DateOnly CheckingDate { get; set; }

    public int AttractionId { get; set; }

    public int EmployeeId { get; set; }

    public string AdmissionLaunch { get; set; } = null!;

    public string? Note { get; set; }

    public virtual Attraction Attraction { get; set; } = null!;

    public virtual Staff Employee { get; set; } = null!;
}
