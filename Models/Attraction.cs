using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSEPractice2.Models;

public partial class Attraction
{
    public int AttractionId { get; set; }

    [Required(ErrorMessage = "Поле Название аттракциона обязательно.")]
    [StringLength(64, ErrorMessage = "Максимальная длина поля Название аттракциона - 64 символа.")]
    public string AttractionName { get; set; } = null!;

    [Required(ErrorMessage = "Поле Описание аттракциона обязательно.")]
    public string AttractionDescription { get; set; } = null!;

    public int? GrowthRestrictions { get; set; }

    public int? WeightRestrictions { get; set; }

    public float? SlideHeight { get; set; }

    public decimal? DescentTime { get; set; }

    [Required(ErrorMessage = "Поле Зона расположения обязательно.")]
    public int ZoneLocation { get; set; }

    //public virtual ICollection<CheckingAttraction> CheckingAttractions { get; set; } = new List<CheckingAttraction>();
	public virtual ICollection<Checking> Checkings { get; set; } = new List<Checking>();
	public virtual Zone? ZoneLocationNavigation { get; set; } = null!;
}
