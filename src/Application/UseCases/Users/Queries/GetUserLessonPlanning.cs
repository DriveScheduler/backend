using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Business;
using Domain.Exceptions.Users;
using Domain.ValueObjects;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserLessonPlanning_Query(Guid UserId) : IRequest<UserLessonPlanning>;

    internal sealed class GetUserLessonPlanning_QueryHandler(IDatabase database, ISystemClock clock) : IRequestHandler<GetUserLessonPlanning_Query, UserLessonPlanning>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _clock = clock;

        public async Task<UserLessonPlanning> Handle(GetUserLessonPlanning_Query request, CancellationToken cancellationToken)
        {
            User? user = _database.Users.Find(request.UserId);
            if(user is null)
                throw new UserNotFoundException();

            List<Lesson> lessons = await _database.Lessons
                .Include(lesson => lesson.Student)
                .Where(lesson => lesson.Student == user && lesson.Start > _clock.Now)
                .OrderBy(lesson => lesson.Start)
                .ToListAsync();

            DateTime tomorrow = _clock.Now.Date.AddDays(1).Date;            
            DateTime lastDayOfThisWeek = DateUtil.GetLastDayOfWeek(_clock.Now);
            DateTime firstDayOfNextWeek = DateUtil.GetFirstDayOfWeek(_clock.Now.AddDays(7));
            DateTime lastDayOfThisMonth = DateUtil.GetLastDayOfMonth(_clock.Now);

            UserLessonPlanning planning = new UserLessonPlanning()
            {
                Today = lessons.Where(lesson => lesson.Start.Date == _clock.Now.Date).ToList(),
                Tomorrow = lessons.Where(lesson => lesson.Start.Date == tomorrow).ToList(),
                ThisWeek = lessons.Where(lesson => lesson.Start.Date > tomorrow && lesson.Start.Date <= lastDayOfThisWeek).ToList(),
                ThisMonth = lessons.Where(lesson => lesson.Start.Date > lastDayOfThisWeek && lesson.Start.Date <= lastDayOfThisMonth).ToList(),
                NextMonths = lessons.Where(lesson => lesson.Start.Date > lastDayOfThisMonth).ToList(),
                TotalLessons = lessons.Count,
                TotalTime = lessons.Sum(lesson => lesson.Duration)
            };            

            return planning;
        }
    }
}
