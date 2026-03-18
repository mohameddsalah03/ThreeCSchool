using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class CartItem : BaseEntity<int>
    {
        public decimal PriceSnapshot { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int CartId { get; set; }

        // Nullable FKs — either Course or Plan, never both
        public int? CourseId { get; set; }
        public int? PricingPlanId { get; set; }

        // Navigation
        public Cart Cart { get; set; } = null!;
        public Course? Course { get; set; }
        public PricingPlan? PricingPlan { get; set; }
    }
}