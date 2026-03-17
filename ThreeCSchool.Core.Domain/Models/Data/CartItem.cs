using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class CartItem : BaseEntity<int>
    {
        public int CartId { get; set; }
        public int? CourseId { get; set; }
        public int? PricingPlanId { get; set; }
        public decimal PriceSnapshot { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Cart Cart { get; set; } = null!;
        public Course? Course { get; set; }
        public PricingPlan? PricingPlan { get; set; }
    }
}