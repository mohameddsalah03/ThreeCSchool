using ThreeCSchool.Core.Domain.Models.Base;
using ThreeCSchool.Core.Domain.Models.Data.Enums;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    /// <summary>
    /// حجز طالب في جلسة معينة
    /// </summary>
    public class Booking : BaseEntity<int>
    {
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;

        // FKs
        public string UserId { get; set; } = null!;
        public int CourseSessionId { get; set; }

        // Navigation
        public ApplicationUser User { get; set; } = null!;
        public CourseSession CourseSession { get; set; } = null!;
    }
}