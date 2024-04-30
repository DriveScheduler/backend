namespace Domain.Exceptions.Lessons
{
    public sealed class LessonValidationException : Exception
    {
        public LessonValidationException(string message) : base(message)
        {
        }
    }
}
