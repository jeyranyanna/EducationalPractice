using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSEPractice2.Models;

public partial class Zone
{
    public int ZoneId { get; set; }

    [Required(ErrorMessage ="Поле Название зоны обязательно.")]
	[StringLength(64, ErrorMessage = "Максимальная длина поля Название зоны - 64 символа.")]
	public string ZoneName { get; set; } = null!;

    public virtual ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
