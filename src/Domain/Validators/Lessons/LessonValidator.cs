using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions.Lessons;

using FluentValidation;

namespace Domain.Validators.Lessons
{
    public sealed class LessonValidator : CustomValidator<Lesson, LessonValidationException>
    {
        public LessonValidator(ISystemClock systemClock)
        {
            RuleFor(lesson => lesson.Name)
                .NotEmpty()
                .WithMessage("Le nom du cours est obligatoire");

            RuleFor(lesson => lesson.Start)
                .NotEmpty()
                .WithMessage("La date est obligatoire");

            RuleFor(lesson => lesson.Start)
                .GreaterThanOrEqualTo(systemClock.Now)
                .WithMessage("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant");

            RuleFor(lesson => lesson.Duration)
                .InclusiveBetween(30, 120)
                .WithMessage("La durée du cours n'est pas valide (elle doit être comprise entre 30min et 2h)");        

            RuleFor(lesson => lesson.Type)
                .Equal(lesson => lesson.Teacher.LicenceType)
                .WithMessage("Le moniteur doit pouvoir assurer ce type de cours");

            RuleFor(lesson => lesson.Type)
                .Equal(lesson => lesson.Vehicle.Type)
                .WithMessage("Le type de vehicule ne correspond pas au type de cours");          


            RuleFor(lesson => lesson)
               .Custom((lesson, context) =>
               {
                   foreach (var teacherLesson in lesson.Teacher.LessonsAsTeacher)
                   {
                       if (teacherLesson.End > lesson.Start && teacherLesson.Start < lesson.End && teacherLesson.Id != lesson.Id)
                       {
                           context.AddFailure("Le moniteur n'est pas disponible pour cette plage horaire");
                           break;
                       }
                   }
               });

            RuleFor(lesson => lesson)
              .Custom((lesson, context) =>
              {
                  foreach (var vehicleLesson in lesson.Vehicle.Lessons)
                  {
                      if (vehicleLesson.End > lesson.Start && vehicleLesson.Start < lesson.End && vehicleLesson.Id != lesson.Id)
                      {
                          context.AddFailure("Le vehicule demandé n'est pas disponible pour cette plage horaire");
                          break;
                      }
                  }
              });
        }
    }
}
