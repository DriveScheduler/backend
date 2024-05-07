using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Validators.Lessons;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record AddStudentToWaitingList_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class AddStudentToWaitingList_CommandHandler(IDatabase database, ISystemClock systemClock) : IRequestHandler<AddStudentToWaitingList_Command>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _systemClock = systemClock;

        public async Task Handle(AddStudentToWaitingList_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            Lesson? lesson = _database.Lessons
                .Include(Lesson => Lesson.Student)
                .Include(Lesson => Lesson.WaitingList)
                .FirstOrDefault(l => l.Id == request.LessonId);
            if (lesson is null)
                throw new LessonNotFoundException();

            new LessonTimeValidator(_systemClock)
                .ThrowIfInvalid(lesson);

            lesson.WaitingList.Add(user);
            new UserLessonValidator().ThrowIfInvalid(lesson);

            if (await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();
        }
    }
}
