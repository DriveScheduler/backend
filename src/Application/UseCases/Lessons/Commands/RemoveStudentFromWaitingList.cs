using Domain.Models;
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

        public async Task Handle(RemoveStudentFromWaitingList_Command request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetUserByIdAsync(request.UserId);
            //User? user = _database.Users.Find(request.UserId);
            //if (user is null)
            //    throw new UserNotFoundException();

            Lesson lesson = await _lessonRepository.GetByIdAsync(request.LessonId);
            //Lesson? lesson = _database.Lessons
            //    .Include(Lesson => Lesson.WaitingList)
            //    .FirstOrDefault(l => l.Id == request.LessonId);
            //if (lesson is null)
            //    throw new LessonNotFoundException();

            lesson.RemoveStudentFromWaitingList(user);
            //lesson.WaitingList.Remove(user);

            await _lessonRepository.UpdateAsync(lesson);
            //if (await _database.SaveChangesAsync() != 1)
            //    throw new LessonSaveException();
        }
    }
}
