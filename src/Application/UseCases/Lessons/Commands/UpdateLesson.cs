using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Exceptions.Vehicles;
using Domain.Validators.Lessons;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{

    public sealed record UpdateLesson_Command(int Id, string Name, DateTime Date, int Duration, Guid TeacherId, LicenceType Type, int VehicleId) : IRequest;

    internal sealed class UpdateLesson_CommandHandler(IDatabase database, ISystemClock clock) : IRequestHandler<UpdateLesson_Command>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _clock = clock;

        public async Task Handle(UpdateLesson_Command request, CancellationToken cancellationToken)
        {
            User teacher = GetTeacher(request.TeacherId);
            Vehicle vehicle = GetVehicle(request.VehicleId);

            Lesson? lesson = _database.Lessons.Find(request.Id);
            if (lesson is null)
                throw new LessonNotFoundException();

            LessonValidator validator = new LessonValidator(lesson, _clock)
                .UpdateRules();            

            lesson.Name = request.Name;
            lesson.Start = request.Date;
            lesson.Duration = request.Duration;
            lesson.Type = request.Type;
            lesson.Teacher = teacher;
            lesson.Vehicle = vehicle;

            validator.ThrowIfInvalid(lesson);

            if (await _database.SaveChangesAsync(cancellationToken) != 1)
                throw new LessonSaveException();
        }

        private User GetTeacher(Guid id)
        {
            User? user = _database.Users
                .Include(u => u.LessonsAsTeacher)
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
