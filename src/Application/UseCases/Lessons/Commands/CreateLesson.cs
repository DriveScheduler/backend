using Application.Abstractions;

using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record CreateLesson_Command(string Name, DateTime Date, int Duration, Guid TeacherId) : IRequest<int>;

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
            Teacher teacher = _userRepository.GetTeacherById(request.TeacherId);
            Vehicle vehicle = _vehicleRepository.FindAvailable(request.Date, request.Duration, teacher.LicenceType);
            
            if (request.Date < _systemClock.Now)
                throw new LessonValidationException("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant");

            DateTime end = request.Date.AddMinutes(request.Duration);
            if (teacher.Lessons.Any(teacherLesson => teacherLesson.Start < end && teacherLesson.End > request.Date))
                throw new LessonValidationException("Le moniteur n'est pas disponible pour cette plage horaire");

            Lesson lesson = new Lesson(
                request.Name,
                request.Date,
                request.Duration,
                teacher,
                teacher.LicenceType,
                vehicle);
       
            int id = _lessonRepository.Insert(lesson);
            return Task.FromResult(id);
        }
    }
}
