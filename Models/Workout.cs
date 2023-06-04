using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Workout
{
    public int WorkoutId { get; set; }

    public DateOnly DateOfWorkout { get; set; }

    public string WorkoutType { get; set; } = null!;

    public TimeOnly Duration { get; set; }

    public virtual ICollection<CurrentWorkout> CurrentWorkouts { get; set; } = new List<CurrentWorkout>();
}
