//using Domain.Entities;
//using Domain.Enums;
//using Domain.Exceptions.Lessons;
//using Domain.Models;

//using FluentValidation;


//namespace Domain.Validators.Lessons
//{
//    public sealed class LessonValidator : CustomValidator<Lesson, LessonValidationException>
//    {
//        private readonly ISystemClock _systemClock;


//        public LessonValidator(ISystemClock systemClock)
//        {
//            _systemClock = systemClock;
//        }

//        public LessonValidator(Lesson initialState, ISystemClock systemClock) : base(initialState)
//        {
//            _systemClock = systemClock;
//        }

//        public LessonValidator CreateRules()
//        {
//            DefaultRules();
//            return this;
//        }

//        public LessonValidator UpdateRules()
//        {
//            RuleFor(lesson => InitialState!.Start)
//                .GreaterThanOrEqualTo(_systemClock.Now)
//                .WithMessage("Le cours est déjà passé");

//            DefaultRules();

//            return this;
//        }

//        public LessonValidator AddStudentRules()
//        {
//            RuleFor(lesson => lesson.Start)
//                .GreaterThanOrEqualTo(_systemClock.Now)
//                .WithMessage("Le cours est déjà passé");

//            RuleFor(lesson => lesson.Student)
//                .Custom((student, context) =>
//                {
//                    if (InitialState is null) return;

//                    if (InitialState.Student is not null)
//                    {
//                        context.AddFailure("Le cours est complet");
//                    }
//                });

//            RuleFor(lesson => lesson.Student)
//                .Custom((student, context) =>
//                {
//                    if (student is null) return;

//                    if (student.Type != UserType.Student)
//                    {
//                        context.AddFailure("L'utilisateur doit être un élève pour s'incrire au cours");
//                    }
//                });

//            RuleFor(lesson => lesson)
//               .Custom((lesson, context) =>
//               {
//                   if (lesson.Student is not null && lesson.Student.LicenceType != lesson.Type)
//                   {
//                       context.AddFailure("Le permis de l'utilisateur ne correspond pas au type de cours");
//                   }
//               });

//            return this;
//        }

//        public LessonValidator AddStudentToWaitingListRules()
//        {
//            RuleFor(lesson => lesson.Start)
//                .GreaterThanOrEqualTo(_systemClock.Now)
//                .WithMessage("Le cours est déjà passé");

//            RuleFor(lesson => lesson)
//               .Custom((lesson, context) =>
//               {
//                   if (lesson.WaitingList is null) return;

//                   if (lesson.WaitingList.Count > 0 && lesson.Student is null)
//                   {
//                       context.AddFailure("Le cours n'est pas complet");
//                   }
//               });

//            RuleFor(lesson => lesson.WaitingList)
//                .Custom((pending, context) =>
//                {
//                    if (pending is null) return;

//                    if (pending.Where(u => u.Type != Enums.UserType.Student).Count() > 0)
//                    {
//                        context.AddFailure("L'utilisateur doit être un élève pour s'incrire à la file d'attente du cours");
//                    }
//                });

//            RuleFor(lesson => lesson.WaitingList)
//                .Custom((waitingList, context) =>
//                {
//                    if (waitingList is null) return;

//                    List<User> distinctList = waitingList.DistinctBy(student => student.Id).ToList();
//                    if (distinctList.Count != waitingList.Count)
//                    {
//                        context.AddFailure("L'utilisateur est déjà dans la liste d'attente");
//                    }
//                });

//            RuleFor(lesson => lesson)
//                .Custom((lesson, context) =>
//                {
//                    if (lesson.WaitingList is not null)
//                    {
//                        foreach (var student in lesson.WaitingList)
//                        {
//                            if (student.LicenceType != lesson.Type)
//                            {
//                                context.AddFailure("Le permis de l'utilisateur ne correspond pas au type de cours");
//                                break;
//                            }
//                        }
//                    }

//                });

//            return this;
//        }

//        private void DefaultRules()
//        {
//            RuleFor(lesson => lesson.Name)
//                .NotEmpty()
//                .WithMessage("Le nom du cours est obligatoire");

//            RuleFor(lesson => lesson.Start)
//                .NotEmpty()
//                .WithMessage("La date est obligatoire");
//            RuleFor(lesson => lesson.Start)
//                .GreaterThanOrEqualTo(_systemClock.Now)
//                .WithMessage("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant");

//            RuleFor(lesson => lesson.Duration)
//                .InclusiveBetween(30, 120)
//                .WithMessage("La durée du cours n'est pas valide (elle doit être comprise entre 30min et 2h)");

//            RuleFor(lesson => lesson.Type)
//                .Equal(lesson => lesson.Teacher.LicenceType)
//                .WithMessage("Le moniteur doit pouvoir assurer ce type de cours");

//            RuleFor(lesson => lesson.Teacher)
//                .Must(teacher => teacher.Type == UserType.Teacher)
//                .WithMessage("La personne en charge du cours doit être un moniteur");

//            RuleFor(lesson => lesson.Type)
//                .Equal(lesson => lesson.Vehicle.Type)
//                .WithMessage("Le type de vehicule ne correspond pas au type de cours");


//            RuleFor(lesson => lesson)
//               .Custom((lesson, context) =>
//               {
//                   foreach (var teacherLesson in lesson.Teacher.LessonsAsTeacher)
//                   {
//                       if (teacherLesson.End > lesson.Start && teacherLesson.Start < lesson.End && teacherLesson.Id != lesson.Id)
//                       {
//                           context.AddFailure("Le moniteur n'est pas disponible pour cette plage horaire");
//                           break;
//                       }
//                   }
//               });

//            RuleFor(lesson => lesson)
//              .Custom((lesson, context) =>
//              {
//                  foreach (var vehicleLesson in lesson.Vehicle.Lessons)
//                  {
//                      if (vehicleLesson.End > lesson.Start && vehicleLesson.Start < lesson.End && vehicleLesson.Id != lesson.Id)
//                      {
//                          context.AddFailure("Le vehicule demandé n'est pas disponible pour cette plage horaire");
//                          break;
//                      }
//                  }
//              });
//        }

//    }
//}
