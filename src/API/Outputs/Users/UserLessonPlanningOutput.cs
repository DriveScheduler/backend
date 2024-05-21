using API.Outputs.Lessons;

using Domain.Entities.Business;

namespace API.Outputs.Users
{
    public sealed class UserLessonPlanningOutput
    {
        public List<LessonLight> Today { get; }
        public List<LessonLight> Tomorrow { get; }
        public List<LessonLight> ThisWeek { get; }
        public List<LessonLight> ThisMonth { get; }
        public List<LessonLight> NextMonths { get; }
        public int TotalLessons { get; init; }
        public int TotalTime { get; init; }

        public UserLessonPlanningOutput(UserLessonPlanning planning)
        {
            Today = planning.Today.Select(lesson => new LessonLight(lesson)).ToList();
            Tomorrow = planning.Tomorrow.Select(lesson => new LessonLight(lesson)).ToList();
            ThisWeek = planning.ThisWeek.Select(lesson => new LessonLight(lesson)).ToList();
            ThisMonth = planning.ThisMonth.Select(lesson => new LessonLight(lesson)).ToList();
            NextMonths = planning.NextMonths.Select(lesson => new LessonLight(lesson)).ToList();
            TotalLessons = planning.TotalLessons;
            TotalTime = planning.TotalTime;
        }

    }
}
