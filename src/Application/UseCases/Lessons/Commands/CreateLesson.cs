using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Validators.Lessons;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record CreateLesson_Command(string Name, DateTime Date, int Duration, Guid TeacherId, LicenceType Type) : IRequest<int>;

    internal sealed class CreateLesson_CommandHandler(IDatabase database, ISystemClock systemClock) : IRequestHandler<CreateLesson_Command, int>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _systemClock = systemClock;

        public async Task<int> Handle(CreateLesson_Command request, CancellationToken cancellationToken)
        {
            User user = GetTeacher(request.TeacherId);
            Vehicle vehicle = FindAvailableVehicle(request.Type, request.Date, request.Date.AddMinutes(request.Duration));

            Lesson lesson = new Lesson()
            {
                Name = request.Name,
                Start = request.Date,
                Duration = request.Duration,
                Type = request.Type,
                Teacher = user,
                Vehicle = vehicle,
            };

            new LessonValidator(_systemClock).ThrowIfInvalid(lesson);

            _database.Lessons.Add(lesson);
            if (await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();

            return lesson.Id;
        }

        private User GetTeacher(Guid id)
        {
            User? user = _database.Users
                .Include(u => u.LessonsAsTeacher)
                .FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new UserNotFoundException();
            if (user.Type != UserType.Teacher)
                throw new LessonValidationException("La personne en charge du cours doit être un moniteur");

            return user;
        }

        private Vehicle FindAvailableVehicle(LicenceType vehicleType, DateTime lessonStart, DateTime lessonEnd)
        {
            List<Vehicle> vehicles = [.. _database.Vehicles
                .Include(v => v.Lessons)
                .Where(v => v.Type == vehicleType)];

            Vehicle? vehicle = vehicles.FirstOrDefault(v => !v.Lessons.Any(lesson => lessonStart < lesson.End || lessonEnd > lesson.Start));
            if (vehicle is null)
                throw new LessonValidationException("Aucun vehicule disponibe pour valider ce cours");

            return vehicle;
        }
    }
}
