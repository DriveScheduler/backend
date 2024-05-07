using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Exceptions.Vehicles;
using Domain.Validators.Lessons;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record CreateLesson_Command(string Name, DateTime Date, int Duration, Guid TeacherId, LicenceType Type, int VehicleId) : IRequest<int>;

    internal sealed class CreateLesson_CommandHandler(IDatabase database, ISystemClock systemClock) : IRequestHandler<CreateLesson_Command, int>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _systemClock = systemClock;

        public async Task<int> Handle(CreateLesson_Command request, CancellationToken cancellationToken)
        {
            User user = GetTeacher(request.TeacherId);
            Vehicle vehicle = GetVehicle(request.VehicleId);

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

        private Vehicle GetVehicle(int id)
        {
            Vehicle? vehicle = _database.Vehicles
                .Include(v => v.Lessons)
                .FirstOrDefault(v => v.Id == id);
            if (vehicle is null)
                throw new VehicleNotFoundException();

            return vehicle;
        }
    }
}
