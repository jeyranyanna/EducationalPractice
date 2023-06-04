using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Shift
{
    public int ShiftId { get; set; }

    public DateOnly ShiftDate { get; set; }

    public int WorkShedule { get; set; }

    public virtual ICollection<CurrentShift> CurrentShifts { get; set; } = new List<CurrentShift>();
}
