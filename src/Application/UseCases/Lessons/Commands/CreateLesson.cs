using Application.Abstractions;

using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record CreateLesson_Command(string Name, DateTime Date, int Duration, Guid TeacherId, LicenceType Type) : IRequest<int>;

    internal sealed class CreateLesson_CommandHandler(
        ILessonRepository lessonRepository,
        IUserRepository userRepository,
        IVehicleRepository vehicleRepository,
        ISystemClock systemClock
        ) : IRequestHandler<CreateLesson_Command, int>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ISystemClock _systemClock = systemClock;

        public Task<int> Handle(CreateLesson_Command request, CancellationToken cancellationToken)
        {
            User teacher = _userRepository.GetUserById(request.TeacherId);
            Vehicle vehicle = _vehicleRepository.FindAvailable(request.Date, request.Duration, request.Type);

            Lesson lesson = new Lesson(
                request.Name,
                request.Date,
                request.Duration,
                teacher,
                request.Type,
                vehicle);

            if (lesson.Start < _systemClock.Now)
                throw new LessonValidationException("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant");

            if (teacher.LessonsAsTeacher.Any(teacherLesson => teacherLesson.Start < lesson.End && teacherLesson.End > lesson.Start))
                throw new LessonValidationException("Le moniteur n'est pas disponible pour cette plage horaire");


            _lessonRepository.Insert(lesson);
            return Task.FromResult(lesson.Id);
        }
    }
}
