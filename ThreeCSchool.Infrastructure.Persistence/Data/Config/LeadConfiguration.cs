using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Core.Domain.Models.Data.Enums;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.ToTable("Leads");

            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.ParentName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.StudentAge).IsRequired();

            builder.Property(l => l.ParentPhone)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(l => l.ParentEmail)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(l => l.School)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(l => l.Notes).IsRequired(false);

            builder.Property(l => l.Status)
                .HasDefaultValue(LeadStatus.New);
        }
    }
}