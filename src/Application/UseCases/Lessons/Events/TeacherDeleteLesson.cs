using Application.Abstractions;

using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Events
{
    internal sealed record TeacherDeleteLesson_Notification(int LessonId) : INotification;
    internal class TeacherDeleteLesson_NotificationHandler(ILessonRepository lessonRepository, IEmailSender email) : INotificationHandler<TeacherDeleteLesson_Notification>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IEmailSender _email = email;

        public Task Handle(TeacherDeleteLesson_Notification notification, CancellationToken cancellationToken)
        {
            Lesson lesson = _lessonRepository.GetById(notification.LessonId);
            List<string> emails = lesson.WaitingList.Select(u => u.Email.Value).ToList();
            if (lesson.Student != null)
                emails.Add(lesson.Student.Email.Value);
            return _email.SendAsync("Annulation du cours", $"Le cours {lesson.Name} a été annulé", emails);
        }
    }
}
