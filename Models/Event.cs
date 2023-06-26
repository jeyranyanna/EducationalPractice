using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Event
{
    public int EventId { get; set; }

    [Required(ErrorMessage ="Поле Название мероприятия обязательно.")]
    [StringLength(64, ErrorMessage ="Максимальная длина поля Название мероприятия - 64 символа.")]
    public string EventName { get; set; } = null!;

    [Required(ErrorMessage = "Поле Дата и время мероприятия обязательно.")]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Поле Продолжительность мероприятия обязательно.")]
    public TimeSpan Duration { get; set; }


    [StringLength(64, ErrorMessage = "Максимальная длина поля Описание мероприятия - 100 символов.")]
    public string? Description { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Staff? Employee { get; set; }
}
