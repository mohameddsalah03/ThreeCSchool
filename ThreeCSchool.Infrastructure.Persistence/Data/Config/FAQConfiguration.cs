using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder.ToTable("FAQs");

            builder.Property(f => f.Id).ValueGeneratedOnAdd();
            builder.Property(f => f.QuestionEn).HasMaxLength(500).IsRequired();
            builder.Property(f => f.QuestionAr).HasMaxLength(500).IsRequired();
            builder.Property(f => f.AnswerEn).IsRequired();
            builder.Property(f => f.AnswerAr).IsRequired();
            builder.Property(f => f.DisplayOrder).HasDefaultValue(0);
            builder.Property(f => f.IsActive).HasDefaultValue(true);
        }
    }
}