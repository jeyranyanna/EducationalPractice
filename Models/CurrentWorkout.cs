using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class CurrentWorkout
{
    public int CurrentWorkoutId { get; set; }

    public int WorkoutDate { get; set; }

    public int Trainer { get; set; }

    public virtual Staff TrainerNavigation { get; set; } = null!;

    public virtual Workout WorkoutDateNavigation { get; set; } = null!;
}
