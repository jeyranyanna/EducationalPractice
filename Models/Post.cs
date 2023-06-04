using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Post1 { get; set; } = null!;

    public int SalaryAmount { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
