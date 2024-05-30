using Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configurations.ValueObjectsConverter
{
    internal class LessonDurationConverter : ValueConverter<LessonDuration, int>
    {
        public LessonDurationConverter()
            : base(duration => duration.Value, value => new LessonDuration(value)) { }
    }
}
