using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Validators.Lessons;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record AddStudent_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class AddStudent_CommandHandler(IDatabase database) : IRequestHandler<AddStudent_Command>
    {
        private readonly IDatabase _database = database;        

        public async Task Handle(AddStudent_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            Lesson? lesson = _database.Lessons
                .Include(Lesson => Lesson.Students)
                .FirstOrDefault(l => l.Id == request.LessonId);
            if (lesson is null)
                throw new LessonNotFoundException();

            lesson.Students.Add(user);
            new UserLessonValidator()
                .ExecuteBeforeThrowing(obj => obj.Students.Remove(user))
                .ThrowIfInvalid(lesson);

            if(await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();
        }
    }
}
