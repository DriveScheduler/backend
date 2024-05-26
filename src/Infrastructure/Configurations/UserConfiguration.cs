using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User_Database>
    {
        public void Configure(EntityTypeBuilder<User_Database> builder)
        {
            builder.Property(u => u.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LicenceType)
                .IsRequired();
        }
    }
}
