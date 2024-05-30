using Domain.Models;

namespace Application.Models
{
    public sealed class UserLessonHistory
    {
        public List<Lesson> Lessons { get; init; }        
        public int LessonTotalTime { get; init; }
    }
}
