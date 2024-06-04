using API.Outputs.Lessons;

using Application.Models;

using Domain.Models.Users;

namespace API.Outputs.Users
{
    public sealed class UserLessonHistoryOutput
    {
        public List<LessonDetail> Lessons { get; }
        public int LessonTotalTime { get; }

        public UserLessonHistoryOutput(UserLessonHistory history, User connectedUser)
        {
            Lessons = history.Lessons.Select(lesson => new LessonDetail(lesson, connectedUser)).ToList();
            LessonTotalTime = history.LessonTotalTime;
        }
    }
}
