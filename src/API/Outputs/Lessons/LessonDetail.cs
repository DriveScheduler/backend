using API.Outputs.Users;

using Domain.Models;

namespace API.Outputs.Lessons
{
    public class LessonDetail : LessonLight
    {
        public LessonStateOutput State { get; }
        public UserLight? Student { get; }
        public int WaitingList { get; }
        public LessonDetail(Lesson lesson, User connectedUser) : base(lesson)
        {
            State = new LessonStateOutput(lesson.State(connectedUser));
            Student = lesson.Student == null ? null : new UserLight(lesson.Student);
            WaitingList = lesson.WaitingList.Count;
        }
    }
}
