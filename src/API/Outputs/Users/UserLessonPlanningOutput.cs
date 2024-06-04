using API.Outputs.Lessons;

using Application.Models;

using Domain.Models.Users;

namespace API.Outputs.Users
{
    public sealed class UserLessonPlanningOutput
    {
        public List<LessonDetail> Today { get; }
        public List<LessonDetail> Tomorrow { get; }
        public List<LessonDetail> ThisWeek { get; }
        public List<LessonDetail> ThisMonth { get; }
        public List<LessonDetail> NextMonths { get; }
        public int TotalLessons { get; init; }
        public int TotalTime { get; init; }

        public UserLessonPlanningOutput(UserLessonPlanning planning, User conectedUser)
        {
            Today = planning.Today.Select(lesson => new LessonDetail(lesson, conectedUser)).ToList();
            Tomorrow = planning.Tomorrow.Select(lesson => new LessonDetail(lesson, conectedUser)).ToList();
            ThisWeek = planning.ThisWeek.Select(lesson => new LessonDetail(lesson, conectedUser)).ToList();
            ThisMonth = planning.ThisMonth.Select(lesson => new LessonDetail(lesson, conectedUser)).ToList();
            NextMonths = planning.NextMonths.Select(lesson => new LessonDetail(lesson, conectedUser)).ToList();
            TotalLessons = planning.TotalLessons;
            TotalTime = planning.TotalTime;
        }

    }
}
