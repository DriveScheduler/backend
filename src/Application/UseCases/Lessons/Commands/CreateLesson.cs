using Application.Abstractions;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
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

        public async Task<int> Handle(CreateLesson_Command request, CancellationToken cancellationToken)
        {
            User teacher = await _userRepository.GetTeacherById(request.TeacherId);
            Vehicle vehicle = await _vehicleRepository.FindAvailable(request.Date, request.Duration, request.Type);
            //User user = GetTeacher(request.TeacherId);
            //Vehicle vehicle = FindAvailableVehicle(request.Type, request.Date, request.Date.AddMinutes(request.Duration));

            Lesson lesson = new Lesson(
                request.Name,
                request.Date,
                request.Duration,
                teacher,
                request.Type,
                vehicle);
            //Lesson lesson = new Lesson()
            //{
            //Name = request.Name,
            //Start = request.Date,
            //Duration = request.Duration,
            //Type = request.Type,
            //Teacher = user,
            //Vehicle = vehicle,
            //};

            //new LessonValidator(_systemClock)
            //    .CreateRules()
            //    .ThrowIfInvalid(lesson);

            //_database.Lessons.Add(lesson);
            //if (await _database.SaveChangesAsync() != 1)
            //    throw new LessonSaveException();

            return await _lessonRepository.InsertAsync(lesson);
            //return lesson.Id;
        }

        //private async Task<User> GetTeacher(Guid id)
        //{
        //    //User? user = _database.Users
        //    //    .Include(u => u.LessonsAsTeacher)
        //    //    .FirstOrDefault(u => u.Id == id);
        //    User teacher = await _userRepository.GetTeacherById(id);
        //    if (teacher is null)
        //        throw new UserNotFoundException();

        //    return teacher;
        //}

        //private async Task<Vehicle> FindAvailableVehicle(LicenceType vehicleType, DateTime lessonStart, DateTime lessonEnd)
        //{
        //    //List<Vehicle> vehicles = [.. _database.Vehicles
        //    //    .Include(v => v.Lessons)
        //    //    .Where(v => v.Type == vehicleType)];
        //    List<Vehicle> vehicles = await _vehicleRepository.GetVehiclesByTypeAsync(vehicleType);

        //    Vehicle? vehicle = vehicles.FirstOrDefault(v => !v.Lessons.Any(lesson => (lesson.End >= lessonStart && lessonStart >= lesson.Start) || (lesson.Start <= lessonEnd && lessonEnd <= lesson.End)));
        //    if (vehicle is null)
        //        throw new LessonValidationException("Aucun vehicule disponibe pour valider ce cours");

        //    return vehicle;
        //}
    }
}
