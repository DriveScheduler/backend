namespace API.Inputs.Lessons
{
    public sealed class UpdateLessonStudentModel
    {
        public int LessonId { get; set; }
        public Guid StudentId { get; set; }
    }
}
