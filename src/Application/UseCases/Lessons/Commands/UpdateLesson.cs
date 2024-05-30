using Application.Abstractions;

using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{

    public sealed record UpdateLesson_Command(int Id, string Name, DateTime Date, int Duration, Guid TeacherId) : IRequest;

    internal sealed class UpdateLesson_CommandHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        IVehicleRepository vehicleRepository,
        ISystemClock clock) : IRequestHandler<UpdateLesson_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ISystemClock _clock = clock;

        public Task Handle(UpdateLesson_Command request, CancellationToken cancellationToken)
        {
            Lesson lesson = _lessonRepository.GetById(request.Id);
            User teacher = _userRepository.GetUserById(request.TeacherId);
            
            if (lesson.Start < _clock.Now)
                throw new LessonValidationException("Le cours est déjà passé");
            if (request.Date < _clock.Now)
                throw new LessonValidationException("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant");

            DateTime end = request.Date.AddMinutes(request.Duration);
            Vehicle vehicle = lesson.Vehicle;
            if (vehicle.Lessons.Any(vehicleLesson => vehicleLesson.Start < end && vehicleLesson.End > request.Date && vehicleLesson.Id != lesson.Id))
                vehicle = _vehicleRepository.FindAvailable(request.Date, request.Duration, lesson.Type);

            if (teacher.LessonsAsTeacher.Any(teacherLesson => teacherLesson.Start < end && teacherLesson.End > request.Date && teacherLesson.Id != lesson.Id))
                throw new LessonValidationException("Le moniteur n'est pas disponible pour cette plage horaire");

            lesson.Update(
                request.Name,
                request.Date,
                request.Duration,
                teacher,
                vehicle);                

            _lessonRepository.Update(lesson);
            return Task.CompletedTask;           
        }
    }
}
