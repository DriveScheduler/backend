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

    public sealed record UpdateLesson_Command(int Id, string Name, DateTime Date, int Duration, Guid TeacherId, LicenceType Type, int VehicleId, int MaxStudent) : IRequest;

    internal sealed class UpdateLesson_CommandHandler(IDatabase database, ISystemClock clock) : IRequestHandler<UpdateLesson_Command>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _clock = clock;

        public async Task Handle(UpdateLesson_Command request, CancellationToken cancellationToken)
        {
            User user = GetUser(request.TeacherId);
            Vehicle vehicle = GetVehicle(request.VehicleId);

            Lesson? lesson = _database.Lessons.Find(request.Id);
            if (lesson is null)
                throw new LessonNotFoundException();

            Lesson model = new Lesson()
            {
                Id = request.Id,
                Name = request.Name,
                Start = request.Date,
                Duration = request.Duration,
                Type = request.Type,
                Teacher = user,
                Vehicle = vehicle,
                MaxStudent = request.MaxStudent
            };

            new LessonValidator(_clock).ThrowIfInvalid(model);

            lesson.Name = model.Name;
            lesson.Start = model.Start;
            lesson.Duration = model.Duration;
            lesson.Type = model.Type;
            lesson.Teacher = model.Teacher;
            lesson.Vehicle = model.Vehicle;
            lesson.MaxStudent = model.MaxStudent;

            if (await _database.SaveChangesAsync(cancellationToken) != 1)
                throw new LessonSaveException();
        }

        private User GetUser(Guid id)
        {
            User? user = _database.Users
                .Include(u => u.Lessons)
                .FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new UserNotFoundException();
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
