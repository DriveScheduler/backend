using Domain.Enums;

namespace API.Inputs.Lessons
{
    public sealed class GetLessonModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool OnlyEmptyLesson { get; set; }
        public List<Guid>? Teachers { get; set; }
    }
}
