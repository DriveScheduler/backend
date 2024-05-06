using Application.UseCases.Lessons.Events;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record RemoveStudent_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class RemoveStudent_CommandHandler(IDatabase database, IMediator mediator) : IRequestHandler<RemoveStudent_Command>
    {
        private readonly IDatabase _database = database;
        private readonly IMediator _mediator = mediator;

        public async Task Handle(RemoveStudent_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            Lesson? lesson = _database.Lessons
                .Include(Lesson => Lesson.Students)
                .FirstOrDefault(l => l.Id == request.LessonId);
            if (lesson is null)
                throw new LessonNotFoundException();

            if(lesson.Students.All(u => u.Id != user.Id))
                throw new LessonValidationException("L'utilisateur n'est pas inscrit à ce cours");

            if(lesson.Students.Count == lesson.MaxStudent)           
                await _mediator.Publish(new StudentLeaveLesson_Notification(request.LessonId), cancellationToken);            

            lesson.Students.Remove(user);

            if(await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();            
        }
    }
}
