using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Staff
{
    public int EmployeeId { get; set; }

	[Required(ErrorMessage = "Фамилия сотрудника обязательна")]
	public string EmployeeSurname { get; set; } = null!;

	[Required(ErrorMessage = "Имя сотрудника обязательно")]
	public string EmployeeName { get; set; } = null!;

	[Required(ErrorMessage = "Отчество сотрудника обязательно")]
	public string EmployeePatronymic { get; set; } = null!;

	[Required(ErrorMessage = "Должность сотрудника обязательна")]
	public int Post { get; set; }

	[Required(ErrorMessage = "Дата найма сотрудника обязательна")]
	[DataType(DataType.Date)]
	public DateTime? DateOfHiring { get; set; }



	public virtual ICollection<CheckingAttraction> CheckingAttractions { get; set; } = new List<CheckingAttraction>();

    public virtual ICollection<CurrentShift> CurrentShifts { get; set; } = new List<CurrentShift>();

    public virtual ICollection<CurrentWorkout> CurrentWorkouts { get; set; } = new List<CurrentWorkout>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual Post PostNavigation { get; set; } = null!;

}
