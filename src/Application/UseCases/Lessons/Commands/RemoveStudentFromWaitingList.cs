using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record RemoveStudentFromWaitingList_Command(int LessonId, Guid UserId) : IRequest;
    internal sealed class RemoveStudentFromWaitingList_CommandHandler(
        ILessonRepository lessonRepository,
        IUserRepository userRepository
        ) : IRequestHandler<RemoveStudentFromWaitingList_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public Task Handle(RemoveStudentFromWaitingList_Command request, CancellationToken cancellationToken)
        {
            Student user = _userRepository.GetStudentById(request.UserId);
            Lesson lesson = _lessonRepository.GetById(request.LessonId);

            lesson.RemoveStudentFromWaitingList(user);

            _lessonRepository.Update(lesson);
            return Task.CompletedTask;
        }
    }
}
