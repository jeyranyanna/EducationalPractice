using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Benefit
{
    public int BenefitId { get; set; }

    [Required(ErrorMessage ="Поле Наименование льготы обязательно.")]
    [StringLength(128, ErrorMessage = "Максимальная длина поля Наименование льготы - 128 символов.")]
    public string BenefitName { get; set; } = null!;

    [Required(ErrorMessage = "Поле Условие для получения льготы обязательно.")]
    [StringLength(256, ErrorMessage = "Максимальная длина поля Условие для получения льготы - 256 символов.")]
    public string ConditionForReceivingBenefit { get; set; } = null!;

    
    [Required(ErrorMessage = "Поле Процент скидки обязательно.")]
    [Range(0,100,ErrorMessage ="Допустимое значение Процента скидки от 0 до 100.")]
	[RegularExpression(@"^\d+$", ErrorMessage = "Поле Процент скидки должно быть целым числом.")]
	public int DiscountPercentage { get; set; }

    public virtual ICollection<OperationsHistory> OperationsHistories { get; set; } = new List<OperationsHistory>();
}
