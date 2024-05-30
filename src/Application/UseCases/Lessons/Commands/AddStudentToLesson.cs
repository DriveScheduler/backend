using Application.Abstractions;

using Domain.Models;
using Domain.Exceptions.Lessons;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record AddStudentToLesson_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class AddStudentToLesson_CommandHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        ISystemClock systemClock
        ) : IRequestHandler<AddStudentToLesson_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _systemClock = systemClock;

        public Task Handle(AddStudentToLesson_Command request, CancellationToken cancellationToken)
        {
            User student = _userRepository.GetUserById(request.UserId);
            Lesson lesson = _lessonRepository.GetById(request.LessonId);
         
            if(lesson.Start < _systemClock.Now)
                throw new LessonValidationException("Le cours est déjà passé");

            lesson.AddStudent(student);          
            _lessonRepository.Update(lesson);

            return Task.CompletedTask;
        }
    }
}

