using Application.Abstractions;

using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Events
{
    internal sealed record StudentLeaveLesson_Notification(int LessonId) : INotification;
    
    internal class StudentLeaveLesson_NotificationHandler(ILessonRepository lessonRepository, IEmailSender email) : INotificationHandler<StudentLeaveLesson_Notification>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IEmailSender _email = email;

        public Task Handle(StudentLeaveLesson_Notification notification, CancellationToken cancellationToken)
        {
            Lesson lesson = _lessonRepository.GetById(notification.LessonId);
            //Lesson? lesson = _database.Lessons.Include(l => l.WaitingList).FirstOrDefault(l => l.Id == notification.LessonId);
            //if (lesson is null)
            //    return;

            _email.SendAsync("Une place vient de se libérer", $"Un élève vient de se désister du cours {lesson.Name}", lesson.WaitingList.Select(u => u.Email.Value).ToList());

            return Task.CompletedTask;
        }
    }
}
