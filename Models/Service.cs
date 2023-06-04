using HSEPractice2.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    [Required(ErrorMessage ="Поле Название услуги обязательно.")]
    [StringLength(60, ErrorMessage ="Максимальная длина поля Название услуги - 60 символов.")]
    public string ServiceName { get; set; } = null!;

    [Required(ErrorMessage = "Поле Описание услуги обязательно.")]
    [StringLength(128, ErrorMessage = "Максимальная длина поля Название услуги - 128 символов.")]
    public string Description { get; set; } = null!;

    //[DisplayName("Цена, руб.")]
    [Required(ErrorMessage = "Поле Цена обязательно.")]
    [Range(1, 100000, ErrorMessage = "Цена должны быть в пределе от 1 до 100.000")]
    [RegularExpression(@"^\d+$", ErrorMessage ="Поле Цена должно быть целым числом.")]
    public int Price { get; set; }

    public virtual ICollection<OperationsHistory> OperationsHistories { get; set; } = new List<OperationsHistory>();
}
