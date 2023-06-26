using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSEPractice2.Models;

public partial class CurrentWorkout
{
    public int CurrentWorkoutId { get; set; }

    [Required(ErrorMessage ="Поле Код тренировки обязательно.")]
    public int WorkoutDate { get; set; }

    public int Trainer { get; set; }

    public virtual Staff? TrainerNavigation { get; set; }

    public virtual Workout? WorkoutDateNavigation { get; set; } = null!;
}
