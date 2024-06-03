using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<UserDataEntity>
    {
        public void Configure(EntityTypeBuilder<UserDataEntity> builder)
        {
            builder.ToTable("Users");

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

            builder.Property(u => u.Password)
               .IsRequired()               
               .HasMaxLength(100);

            builder.Property(u => u.LicenceType)
                .IsRequired();

            builder.Property(u => u.Type)
                .IsRequired();

            builder.Ignore(u => u.LessonsAsStudentId);
            builder.Ignore(u => u.LessonsAsTeacherId);
            builder.Ignore(u => u.LessonWaitingListId);
        }
    }
}
