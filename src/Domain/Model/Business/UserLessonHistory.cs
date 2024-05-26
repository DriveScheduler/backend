namespace Domain.Entities.Business
{
    public sealed class UserLessonHistory
    {
        public List<Lesson> Lessons { get; init; }        
        public int LessonTotalTime { get; init; }
    }
}
