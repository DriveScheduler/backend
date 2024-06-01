using Domain.Models;

using Infrastructure.Configurations.ValueObjectsConverter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasConversion<SurnameConverter>()
                .HasMaxLength(50);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasConversion<FirstnameConverter>()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasConversion<EmailConverter>()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
               .IsRequired()
               .HasConversion<PasswordConverter>()
               .HasMaxLength(100);

            builder.Property(u => u.LicenceType)
                .IsRequired();

            builder.Property(u => u.Type)
                .IsRequired();
        }
    }
}
