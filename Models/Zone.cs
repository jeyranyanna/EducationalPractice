using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSEPractice2.Models;

public partial class Zone
{
    public int ZoneId { get; set; }

    public string ZoneName { get; set; } = null!;

    public virtual ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
