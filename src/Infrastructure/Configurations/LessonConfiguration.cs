using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Start)
                .IsRequired();

            builder.Property(c => c.Duration)
                .IsRequired();               

            builder.Property(c => c.Type)
                .IsRequired();

            builder
                .HasOne(c => c.Teacher)
                .WithMany(u => u.Lessons);

            builder
                .HasOne(c => c.Vehicle)
                .WithMany(v => v.Lessons);

            builder.Ignore(c => c.End);
        }
    }
}
