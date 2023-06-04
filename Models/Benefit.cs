using System;
using System.Collections.Generic;

namespace HSEPractice2.Models;

public partial class Benefit
{
    public int BenefitId { get; set; }

    public string BenefitName { get; set; } = null!;

    public string ConditionForReceivingBenefit { get; set; } = null!;

    public int DiscountPercentage { get; set; }

    public virtual ICollection<OperationsHistory> OperationsHistories { get; set; } = new List<OperationsHistory>();
}
