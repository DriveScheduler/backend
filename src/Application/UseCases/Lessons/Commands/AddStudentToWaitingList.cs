using Application.Abstractions;

using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record AddStudentToWaitingList_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class AddStudentToWaitingList_CommandHandler(
        ILessonRepository lessonRepository,
        IUserRepository userRepository,
        ISystemClock systemClock) : IRequestHandler<AddStudentToWaitingList_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _systemClock = systemClock;

        public Task Handle(AddStudentToWaitingList_Command request, CancellationToken cancellationToken)
        {
            User student = _userRepository.GetUserById(request.UserId);
            Lesson lesson = _lessonRepository.GetById(request.LessonId);

            if (lesson.Start < _systemClock.Now)
                throw new LessonValidationException("Le cours est déjà passé");

            lesson.AddStudentToWaitingList(student);

            _lessonRepository.Update(lesson);
            return Task.CompletedTask;
        }
    }
}
