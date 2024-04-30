namespace Domain.Exceptions.Lessons
{
    public sealed class LessonNotFoundException : Exception
    {
        public LessonNotFoundException() : base("Le cours n'existe pas")
        {
        }
    }
}
