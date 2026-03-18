using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class PricingPlan : BaseEntity<int>
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? SubtitleEn { get; set; }
        public string? SubtitleAr { get; set; }
        public int DurationInMonths { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int LevelCompletionCount { get; set; }
        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        // Navigation
        public ICollection<PlanFeature> Features { get; set; } = new List<PlanFeature>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}