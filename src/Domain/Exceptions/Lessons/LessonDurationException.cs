namespace Domain.Exceptions.Lessons
{
    public sealed class LessonDurationException : Exception
    {
        public LessonDurationException(int min, int max) : base($"La durée doit être comprise entre {min} et {max} minutes") { }
    }
}
