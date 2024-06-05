using Application.Abstractions;
using Application.Models;

using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserLessonPlanning_Query(Guid UserId) : IRequest<UserLessonPlanning>;

    internal sealed class GetUserLessonPlanning_QueryHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        ISystemClock clock
        ) : IRequestHandler<GetUserLessonPlanning_Query, UserLessonPlanning>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _clock = clock;

        public Task<UserLessonPlanning> Handle(GetUserLessonPlanning_Query request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserById(request.UserId);

            DateTime tomorrow = _clock.Now.Date.AddDays(1).Date;
            DateTime lastDayOfThisWeek = GetLastDayOfWeek(_clock.Now);            
            DateTime firstDayOfNextWeek = GetFirstDayOfWeek(_clock.Now.AddDays(7));
            DateTime lastDayOfThisMonth = GetLastDayOfMonth(_clock.Now);

            List<Lesson> lessons = [];
            if (user is Student student)
                lessons = _lessonRepository.GetLessonsForStudent(student);
            else if (user is Teacher teacher)
                lessons = _lessonRepository.GetLessonsForTeacher(teacher);

            lessons = lessons
                .Where(lesson => lesson.Start > _clock.Now)
                .OrderBy(lesson => lesson.Start)
                .ToList();

            UserLessonPlanning planning = new UserLessonPlanning()
            {
                Today = lessons.Where(lesson => lesson.Start.Date == _clock.Now.Date).ToList(),
                Tomorrow = lessons.Where(lesson => lesson.Start.Date == tomorrow).ToList(),
                ThisWeek = lessons.Where(lesson => lesson.Start.Date > tomorrow && lesson.Start.Date <= lastDayOfThisWeek).ToList(),
                ThisMonth = lessons.Where(lesson => lesson.Start.Date > lastDayOfThisWeek && lesson.Start.Date <= lastDayOfThisMonth).ToList(),
                NextMonths = lessons.Where(lesson => lesson.Start.Date > lastDayOfThisMonth).ToList(),
                TotalLessons = lessons.Count,
                TotalTime = lessons.Sum(lesson => lesson.Duration.Value)
            };

            return Task.FromResult(planning);
        }     

        private DateTime GetLastDayOfWeek(DateTime date)
        {
            return date.AddDays((DayOfWeek.Friday - date.DayOfWeek)).Date;
        }

        private DateTime GetFirstDayOfWeek(DateTime date)
        {
            return date.AddDays(-1 * (date.DayOfWeek - DayOfWeek.Monday)).Date;
        }

        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, daysInMonth);
        }
    }
}
