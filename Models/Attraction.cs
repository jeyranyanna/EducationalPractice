using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Attraction
{
    public int AttractionId { get; set; }

    public string AttractionName { get; set; } = null!;

    public string AttractionDescription { get; set; } = null!;

    public int? GrowthRestrictions { get; set; }

    public int? WeightRestrictions { get; set; }

    public decimal? SlideHeight { get; set; }

    public decimal? DescentTime { get; set; }

    public int ZoneLocation { get; set; }

    public virtual ICollection<CheckingAttraction> CheckingAttractions { get; set; } = new List<CheckingAttraction>();

    public virtual Zone ZoneLocationNavigation { get; set; } = null!;
}
