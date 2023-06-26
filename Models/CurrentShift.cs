using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class CurrentShift
{
    public int CurrentShiftId { get; set; }

    [Required(ErrorMessage ="Поле Дата текущей смены обязательно.")]
    public int Shift { get; set; }

    public int? Employee { get; set; }

    public virtual Staff? EmployeeNavigation { get; set; } 

    public virtual Shift? ShiftNavigation { get; set; } = null!;
}
