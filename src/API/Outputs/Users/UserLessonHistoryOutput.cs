using API.Outputs.Lessons;

using Domain.Entities.Business;

namespace API.Outputs.Users
{
    public sealed class UserLessonHistoryOutput
    {
        public List<LessonLight> Lessons { get; }
        public int LessonTotalTime { get; }

        public UserLessonHistoryOutput(UserLessonHistory history)
        {
            Lessons = history.Lessons.Select(lesson => new LessonLight(lesson)).ToList();
            LessonTotalTime = history.LessonTotalTime;
        }
    }
}
