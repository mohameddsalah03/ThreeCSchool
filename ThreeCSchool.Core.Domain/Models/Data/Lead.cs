using ThreeCSchool.Core.Domain.Models.Base;
using ThreeCSchool.Core.Domain.Models.Data.Enums;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class Lead : BaseEntity<int>
    {
        public string ParentName { get; set; } = null!;
        public int StudentAge { get; set; }
        public string ParentPhone { get; set; } = null!;
        public string ParentEmail { get; set; } = null!;
        public string? School { get; set; }
        public string? Notes { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.New;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}