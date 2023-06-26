using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSEPractice2.Models;

public partial class Post
{
    public int PostId { get; set; }

	[Required(ErrorMessage = "Поле Должность обязательно.")]
	[StringLength(60, ErrorMessage = "Максимальная длина поля - 64 символа.")]
	public string Post1 { get; set; } = null!;

	[Required(ErrorMessage = "Поле Заработная плата обязательно.")]
	[Range(13890, 250000, ErrorMessage = "Зарплата должна быть не ниже уровня МРОТ=13890 и не выше 250000р.")]
	[RegularExpression(@"^\d+$", ErrorMessage = "Поле Заработная плата должно быть целым числом.")]
	public int SalaryAmount { get; set; }


    [ForeignKey("Post")]
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

}
