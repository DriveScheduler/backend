using Application.Abstractions;
using Application.UseCases.Lessons.Events;

using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record RemoveStudentFromLesson_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class RemoveStudentFromLesson_CommandHandler(
        ILessonRepository lessonRepository,
        IUserRepository userRepository,
        IMediator mediator,
        ISystemClock systemClock) : IRequestHandler<RemoveStudentFromLesson_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMediator _mediator = mediator;
        private readonly ISystemClock _systemClock = systemClock;

        public Task Handle(RemoveStudentFromLesson_Command request, CancellationToken cancellationToken)
        {
            Student student = _userRepository.GetStudentById(request.UserId);
            Lesson lesson = _lessonRepository.GetById(request.LessonId);

            if (lesson.Start.AddHours(-24) < _systemClock.Now)
                throw new LessonValidationException("Il n'est pas possible de se désincrire moins de 24h avant le début du cours");            

            bool hasStudent = lesson.Student is not null;

            if(hasStudent && student.Id != lesson.Student.Id)
                throw new LessonValidationException("Vous n'êtes pas inscrit à ce cours");

            lesson.RemoveStudent();

            if (hasStudent)
                _mediator.Publish(new StudentLeaveLesson_Notification(request.LessonId), cancellationToken);

            _lessonRepository.Update(lesson);
            return Task.CompletedTask;
        }
    }
}
