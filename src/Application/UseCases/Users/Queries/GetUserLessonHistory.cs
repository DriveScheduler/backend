using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Business;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserLessonHistory_Query(Guid UserId) : IRequest<UserLessonHistory>;

    internal sealed class GetUserLessonHistory_QueryHandler(IDatabase database, ISystemClock clock) : IRequestHandler<GetUserLessonHistory_Query, UserLessonHistory>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _clock = clock;

        public async Task<UserLessonHistory> Handle(GetUserLessonHistory_Query request, CancellationToken cancellationToken)
        {
            User? student = _database.Users.Find(request.UserId);
            if(student is null)
                throw new UserNotFoundException();

            List<Lesson> lessons = await _database.Lessons
                .Include(lesson => lesson.Student)                
                .Where(lesson => lesson.Student == student && (lesson.Start.AddMinutes(lesson.Duration)) <= _clock.Now)
                .OrderByDescending(lesson => lesson.Start)
                .ToListAsync();

            UserLessonHistory history = new UserLessonHistory()
            {
                Lessons = lessons,                
                LessonTotalTime = lessons.Sum(lesson => lesson.Duration)
            };
            return history;
        }
    }
}
