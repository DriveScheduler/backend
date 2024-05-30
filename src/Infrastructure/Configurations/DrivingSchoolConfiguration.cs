using Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class DrivingSchoolConfiguration : IEntityTypeConfiguration<DrivingSchool>
    {
        public void Configure(EntityTypeBuilder<DrivingSchool> builder)
        {
            builder.ToTable("DrvingSchools");

            builder
                .Property(d => d.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(d => d.Address)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
