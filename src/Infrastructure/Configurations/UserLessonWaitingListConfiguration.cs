using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class UserLessonWaitingListConfiguration : IEntityTypeConfiguration<UserLessonWaitingList>
    {
        public void Configure(EntityTypeBuilder<UserLessonWaitingList> builder)
        {
            builder.ToTable("LessonUsersPending");

            builder.HasKey(e => new { e.UserId, e.LessonId });

            builder
                .HasOne(e => e.User)
                .WithMany(user => user.LessonWaitingLists)
                .HasForeignKey(e => e.UserId);

            builder
                .HasOne(e => e.Lesson)
                .WithMany(lesson => lesson.UserWaitingLists)
                .HasForeignKey(e => e.LessonId);
        }
    }
}
