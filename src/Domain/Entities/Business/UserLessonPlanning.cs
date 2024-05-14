using Domain.Entities.Database;

namespace Domain.Entities.Business
{
    public sealed record UserLessonPlanning
    {
        public List<Lesson> Today { get; init; }
        public List<Lesson> Tomorrow { get; init; }
        public List<Lesson> ThisWeek { get; init; }
        public List<Lesson> ThisMonth { get; init; }
        public List<Lesson> NextMonths { get; init; }    
        public int TotalLessons { get; init; }
        public int TotalTime { get; init; }
    }
}
