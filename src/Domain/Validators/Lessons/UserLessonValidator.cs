using Domain.Entities;
using Domain.Exceptions.Lessons;

using FluentValidation;

namespace Domain.Validators.Lessons
{
    public sealed class UserLessonValidator : CustomValidator<Lesson, LessonValidationException>
    {
        public UserLessonValidator()
        {
            RuleFor(lesson => lesson)
                .Custom((lesson, context) =>
                {
                    if (lesson.WaitingList is null) return;

                    if (lesson.WaitingList.Count > 0 && lesson.Student is null)
                    {
                        context.AddFailure("Le cours n'est pas complet");
                    }
                });

            RuleFor(lesson => lesson.Student)
               .Custom((student, context) =>
               {
                   if (student is null) return;

                   if (student.Type != Enums.UserType.Student)
                   {
                       context.AddFailure("L'utilisateur doit être un élève pour s'incrire au cours");
                   }
               });

            RuleFor(lesson => lesson.WaitingList)
           .Custom((pending, context) =>
           {
               if (pending is null) return;

               if (pending.Where(u => u.Type != Enums.UserType.Student).Count() > 0)
               {
                   context.AddFailure("L'utilisateur doit être un élève pour s'incrire à la file d'attente du cours");
               }
           });


            RuleFor(lesson => lesson.WaitingList)
             .Custom((waitingList, context) =>
             {
                 if (waitingList is null) return;

                 List<User> distinctList = waitingList.DistinctBy(student => student.Id).ToList();
                 if (distinctList.Count != waitingList.Count)
                 {
                     context.AddFailure("L'utilisateur est déjà dans la liste d'attente");
                 }
             });

            RuleFor(lesson => lesson)
                .Custom((lesson, context) =>
                {
                    if (lesson.Student is not null && lesson.Student.LicenceType != lesson.Type)
                    {                       
                        context.AddFailure("Le permis de l'utilisateur ne correspond pas au type de cours");                            
                    }
                    if (lesson.WaitingList is not null)
                    {
                        foreach (var student in lesson.WaitingList)
                        {
                            if (student.LicenceType != lesson.Type)
                            {
                                context.AddFailure("Le permis de l'utilisateur ne correspond pas au type de cours");
                                break;
                            }
                        }
                    }

                });
        }
    }
}
