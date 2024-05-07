﻿using Domain.Abstractions;
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

    public sealed record UpdateLesson_Command(int Id, string Name, DateTime Date, int Duration, Guid TeacherId, LicenceType Type, int VehicleId) : IRequest;

    internal sealed class UpdateLesson_CommandHandler(IDatabase database, ISystemClock clock) : IRequestHandler<UpdateLesson_Command>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _clock = clock;

        public async Task Handle(UpdateLesson_Command request, CancellationToken cancellationToken)
        {
            User user = GetTeacher(request.TeacherId);
            Vehicle vehicle = GetVehicle(request.VehicleId);

            Lesson? lesson = _database.Lessons.Find(request.Id);
            if (lesson is null)
                throw new LessonNotFoundException();

            new LessonTimeValidator(_clock).ThrowIfInvalid(lesson);

            Lesson model = new Lesson()
            {
                Id = request.Id,
                Name = request.Name,
                Start = request.Date,
                Duration = request.Duration,
                Type = request.Type,
                Teacher = user,
                Vehicle = vehicle,                
            };            

            new LessonValidator(_clock).ThrowIfInvalid(model);

            lesson.Name = model.Name;
            lesson.Start = model.Start;
            lesson.Duration = model.Duration;
            lesson.Type = model.Type;
            lesson.Teacher = model.Teacher;
            lesson.Vehicle = model.Vehicle;            

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
