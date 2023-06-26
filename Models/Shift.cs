using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Shift
{
    public int ShiftId { get; set; }

    [Required(ErrorMessage ="Поле Дата смены обязательно.")]
	public DateTime ShiftDate { get; set; }

    [Required(ErrorMessage ="Поле Продолжительность рабочего дня обязательно.")]
    [Range(1, 8, ErrorMessage = "Продолжительность рабочего дня от 1ч до 8ч.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Поле Продолжительность рабочего дня должно быть целым числом.")]
    public int WorkShedule { get; set; }

    public virtual ICollection<CurrentShift> CurrentShifts { get; set; } = new List<CurrentShift>();
}
