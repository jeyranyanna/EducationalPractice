using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class Workout
{
    public int WorkoutId { get; set; }

    [Required(ErrorMessage = "Поле Дата тренировки обязательно.")]
    public DateTime DateOfWorkout { get; set; }

    [Required(ErrorMessage = "Поле Тип тренировки обязательно.")]
    [StringLength(64, ErrorMessage = "Максимальная длина поля Тип тренировки - 64 символа.")]
    public string WorkoutType { get; set; } = null!;

    [Required(ErrorMessage = "Поле Продолжительность тренировки обязательно. ")]
    public TimeSpan Duration { get; set; }

    public virtual ICollection<CurrentWorkout> CurrentWorkouts { get; set; } = new List<CurrentWorkout>();
}
