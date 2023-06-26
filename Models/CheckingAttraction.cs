//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace HSEPractice2.Models;

//public partial class CheckingAttraction //доб проверку на уникальность-дата-аттракцион
//{
//    [Required(ErrorMessage ="Поле Дата проверки обязательно.")]
//    public DateTime CheckingDate { get; set; } //Было dateonly

//    [Required(ErrorMessage = "Поле Аттракцион обязательно.")]
//    public int AttractionId { get; set; }

//    public int? EmployeeId { get; set; } // может быть null при удалении

//    public string AdmissionLaunch { get; set; } = null!;

//    public string? Note { get; set; }

//    public virtual Attraction? Attraction { get; set; } = null!;

//    public virtual Staff? Employee { get; set; }
//}
