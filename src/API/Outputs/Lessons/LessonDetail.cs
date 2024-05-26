using Domain.Entities;

namespace API.Outputs.Lessons
{
    public class LessonDetail : LessonLight
    {
        public LessonStateOutput State { get; }
        public LessonDetail(Lesson lesson, User connectedUser) : base(lesson)
        {
            State = new LessonStateOutput(lesson.State(connectedUser));
        }
    }
}
