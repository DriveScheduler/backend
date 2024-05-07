using Application.UseCases.Lessons.Events;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record RemoveStudentFromLesson_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class RemoveStudentFromLesson_CommandHandler(IDatabase database, IMediator mediator, ISystemClock systemClock) : IRequestHandler<RemoveStudentFromLesson_Command>
    {
        private readonly IDatabase _database = database;
        private readonly IMediator _mediator = mediator;
        private readonly ISystemClock _systemClock = systemClock;

        public async Task Handle(RemoveStudentFromLesson_Command request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if (user is null)
                throw new UserNotFoundException();

            Lesson? lesson = _database.Lessons
                .Include(Lesson => Lesson.Student)
                .FirstOrDefault(l => l.Id == request.LessonId);
            if (lesson is null)
                throw new LessonNotFoundException();

            if(lesson.Start.AddHours(-24) < _systemClock.Now)
                throw new LessonValidationException("Il n'est pas possible de se désincrire moins de 24h avant le début du cours");          

            if(lesson.Student is not null)           
                await _mediator.Publish(new StudentLeaveLesson_Notification(request.LessonId), cancellationToken);

            lesson.Student = null;

            if(await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();            
        }
    }
}
