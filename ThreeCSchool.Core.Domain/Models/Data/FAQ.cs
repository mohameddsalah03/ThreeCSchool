using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class FAQ : BaseEntity<int>
    {
        public string QuestionEn { get; set; } = null!;
        public string QuestionAr { get; set; } = null!;
        public string AnswerEn { get; set; } = null!;
        public string AnswerAr { get; set; } = null!;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}