using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class PlanFeature : BaseEntity<int>
    {
        public string DescriptionEn { get; set; } = null!;
        public string DescriptionAr { get; set; } = null!;
        public bool IsIncluded { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        // FK
        public int PricingPlanId { get; set; }

        // Navigation
        public PricingPlan PricingPlan { get; set; } = null!;
    }
}