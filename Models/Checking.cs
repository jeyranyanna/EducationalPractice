using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Checking
{
	public int CheckingId { get; set; }

	[Required(ErrorMessage = "Поле Дата проверки обязательно.")]
	public DateTime CheckingDate { get; set; }

	[Required(ErrorMessage = "Поле Аттракцион обязательно.")]
	public int AttractionId { get; set; }

    public int? EmployeeId { get; set; }

    public string AdmissionLaunch { get; set; } = null!;

    public string? Note { get; set; }

    public virtual Attraction? Attraction { get; set; } = null!;

    public virtual Staff? Employee { get; set; }
}
