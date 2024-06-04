using Application.UseCases.Lessons.Events;

using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record DeleteLesson_Command(int LessonId, Guid UserId) : IRequest;

    internal class DeleteLesson_CommandHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        IMediator mediator
        ) : IRequestHandler<DeleteLesson_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMediator _mediator = mediator;

        public Task Handle(DeleteLesson_Command request, CancellationToken cancellationToken)
        {
            Lesson lesson = _lessonRepository.GetById(request.LessonId); 
            Teacher teacher = _userRepository.GetTeacherById(request.UserId);

            if(lesson.Teacher.Id != teacher.Id)
                throw new LessonValidationException("Vous n'êtes pas le professeur de ce cours");

            _mediator.Publish(new TeacherDeleteLesson_Notification(request.LessonId), cancellationToken);

            _lessonRepository.Delete(lesson);

            return Task.CompletedTask;
        }
    }
}
