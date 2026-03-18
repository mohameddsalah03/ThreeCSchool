using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    /// <summary>
    /// تمثل جلسة حية واحدة — Live Class أو Reattention Session
    /// كل جلسة ليها وقت محدد + مدرس + حد أقصى للطلاب
    /// </summary>
    public class CourseSession : BaseEntity<int>
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentBookings { get; set; } = 0;
        public string? SessionLink { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FKs
        public int CourseId { get; set; }
        public string InstructorId { get; set; } = null!;

        // Navigation
        public Course Course { get; set; } = null!;
        public ApplicationUser Instructor { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}