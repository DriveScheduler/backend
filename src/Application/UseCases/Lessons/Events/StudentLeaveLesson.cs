using Domain.Abstractions;
using Domain.Entities.Database;
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Events
{
    internal sealed record StudentLeaveLesson_Notification(int LessonId) : INotification;
    
    internal class StudentLeaveLesson_NotificationHandler(IDatabase database, IEmailSender email) : INotificationHandler<StudentLeaveLesson_Notification>
    {
        private readonly IDatabase _database = database;
        private readonly IEmailSender _email = email;

        public async Task Handle(StudentLeaveLesson_Notification notification, CancellationToken cancellationToken)
        {
            Lesson? lesson = _database.Lessons.Include(l => l.WaitingList).FirstOrDefault(l => l.Id == notification.LessonId);
            if (lesson is null)
                return;

            await _email.SendAsync("Une place vient de se libérer", $"Un élève vient de se désister du cours {lesson.Name}", lesson.WaitingList.Select(u => u.Email).ToList());
        }
    }
}
