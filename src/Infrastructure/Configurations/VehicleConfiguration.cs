using Domain.Models;

using Infrastructure.Configurations.ValueObjectsConverter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.Property(v => v.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(v => v.RegistrationNumber)
                .HasConversion<RegistrationNumberConverter>()
                .IsRequired();

            builder.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Type)
                .IsRequired();
        }
    }
}
