using Application.Abstractions;

using Domain.Models;
using Domain.Exceptions.Users;
using Domain.Exceptions.Vehicles;
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

        public async Task Handle(UpdateLesson_Command request, CancellationToken cancellationToken)
        {
            User teacher = await _userRepository.GetTeacherById(request.TeacherId);
            Vehicle vehicle = await _vehicleRepository.FindAvailable(request.Date, request.Duration);
            //User teacher = GetTeacher(request.TeacherId);
            //Vehicle vehicle = GetVehicle(request.VehicleId);

            Lesson lesson = await _lessonRepository.GetByIdAsync(request.Id);
            //Lesson? lesson = _database.Lessons.Find(request.Id);
            //if (lesson is null)
            //    throw new LessonNotFoundException();

            lesson.Update(
                request.Name,
                request.Date,
                request.Duration,
                teacher,
                vehicle);
            //LessonValidator validator = new LessonValidator(lesson, _clock)
            //    .UpdateRules();            

            //lesson.Name = request.Name;
            //lesson.Start = request.Date;
            //lesson.Duration = request.Duration;
            //lesson.Type = request.Type;
            //lesson.Teacher = teacher;
            //lesson.Vehicle = vehicle;

            //validator.ThrowIfInvalid(lesson);

            await _lessonRepository.UpdateAsync(lesson);
            //if (await _database.SaveChangesAsync(cancellationToken) != 1)
            //    throw new LessonSaveException();
        }

        //private User GetTeacher(Guid id)
        //{
        //    User? user = _database.Users
        //        .Include(u => u.LessonsAsTeacher)
        //        .FirstOrDefault(u => u.Id == id);
        //    if (user is null)
        //        throw new UserNotFoundException();

        //    return user;
        //}

        //private Vehicle GetVehicle(int id)
        //{
        //    Vehicle? vehicle = _database.Vehicles
        //        .Include(v => v.Lessons)
        //        .FirstOrDefault(v => v.Id == id);
        //    if (vehicle is null)
        //        throw new VehicleNotFoundException();

        //    return vehicle;
        //}
    }
}
