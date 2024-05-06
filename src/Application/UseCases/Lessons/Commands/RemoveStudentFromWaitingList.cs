using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record RemoveStudentFromWaitingList_Command(int LessonId, Guid UserId) : IRequest;
    internal sealed class RemoveStudentFromWaitingList_CommandHandler(IDatabase database) : IRequestHandler<RemoveStudentFromWaitingList_Command>
    {
        private readonly IDatabase _database = database;

        public async Task Handle(RemoveStudentFromWaitingList_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            Lesson? lesson = _database.Lessons
                .Include(Lesson => Lesson.WaitingList)
                .FirstOrDefault(l => l.Id == request.LessonId);
            if (lesson is null)
                throw new LessonNotFoundException();

            lesson.WaitingList.Remove(user);
            if (await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();
        }
    }
}
