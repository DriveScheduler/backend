using Domain.Exceptions.Lessons;

namespace Domain.ValueObjects
{
    public sealed record LessonDuration
    {
        private const int MIN_DURATION = 30;
        private const int MAX_DURATION = 120;
        public int Value { get; }

        public LessonDuration(int value)
        {
            if (value < MIN_DURATION || value > MAX_DURATION)
                throw new LessonDurationException(MIN_DURATION, MAX_DURATION);
            Value = value;
        }

    }
}
