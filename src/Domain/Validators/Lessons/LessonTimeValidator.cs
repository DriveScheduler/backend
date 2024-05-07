using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;

using FluentValidation;

namespace Domain.Validators.Lessons
{
    public sealed class LessonTimeValidator : CustomValidator<Lesson, LessonValidationException>
    {

        public LessonTimeValidator(ISystemClock clock)
        {
            RuleFor(lesson => lesson.Start)
                .GreaterThanOrEqualTo(clock.Now)
                .WithMessage("Le cours est déjà passé");
        }

    }
}
