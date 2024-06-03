using Domain.Models;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class LessonConfiguration : IEntityTypeConfiguration<LessonDataEntity>
    {
        public void Configure(EntityTypeBuilder<LessonDataEntity> builder)
        {
            builder.ToTable("Lessons");

            builder.Property(lesson => lesson.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(lesson => lesson.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(lesson => lesson.Start)
                .IsRequired();

            builder.Property(lesson => lesson.Duration)
                .IsRequired();

            builder.Property(lesson => lesson.Type)
                .IsRequired();

            builder
                .HasOne(lesson => lesson.Teacher)
                .WithMany(user => user.LessonsAsTeacher)
                .HasForeignKey(lesson => lesson.TeacherId);

            builder
                .HasOne(lesson => lesson.Vehicle)
                .WithMany(vehicle => vehicle.Lessons)
                .HasForeignKey(lesson => lesson.VehicleId);

            builder
                .HasOne(lesson => lesson.Student)
                .WithMany(user => user.LessonsAsStudent)
                .HasForeignKey(lesson => lesson.StudentId);

            builder
                .HasMany(lesson => lesson.UserWaitingList)
                .WithMany(user => user.LessonWaitingList)
                .UsingEntity(j => j.ToTable("LessonUsersPending"));
            //.UsingEntity(
            //   "LessonUsersPending",
            //   left => left.HasOne(typeof(UserDataEntity)).WithMany().HasForeignKey(nameof(UserDataEntity.LessonWaitingListId)).HasPrincipalKey(nameof(LessonDataEntity.Id)),
            //   right => right.HasOne(typeof(LessonDataEntity)).WithMany().HasForeignKey(nameof(LessonDataEntity.UserWaitingListId)).HasPrincipalKey(nameof(UserDataEntity.Id)),
            //   join => join.HasKey("LessonId", "UserId"));


            builder.Ignore(lesson => lesson.UserWaitingListId);
        }
    }
}
