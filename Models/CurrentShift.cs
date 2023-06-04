using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class CurrentShift
{
    public int CurrentShiftId { get; set; }

    public int Shift { get; set; }

    public int Employee { get; set; }

    public virtual Staff EmployeeNavigation { get; set; } = null!;

    public virtual Shift ShiftNavigation { get; set; } = null!;
}
