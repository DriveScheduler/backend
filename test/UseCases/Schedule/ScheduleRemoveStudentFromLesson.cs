using Application.Abstractions;
using Application.UseCases.Lessons.Commands;

using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleRemoveStudentFromLesson
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;
        
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;

        public ScheduleRemoveStudentFromLesson()
        {
            SetupDependencies fixture = new SetupDependencies();
            fixture
                .AddDefaultDependencies()
                .AddFakeEmailSender()
                .Build();

            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }
      

        [Fact]
        public async void ScheduleShould_RemoveStudentFromLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            DateTime lessonStart = _clock.Now.AddHours(24);
            const int lessonId = 1;

            var teacher = DataTestFactory.GetCarTeacher(teacherId);
            var student1 = DataTestFactory.GetCarStudent(studentId1);
            var car = DataTestFactory.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car, student1));

            // Act
            await _mediator.Send(new RemoveStudentFromLesson_Command(lessonId, student1.Id));
            Lesson lesson = _lessonRepository.GetById(lessonId);

            // Assert
            Assert.Null(lesson.Student);
        }

        [Fact]
        public async void ScheduleShould_NotRemoveStudentFromLesson_WhenLessonStartIn24h()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            DateTime lessonStart = _clock.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
            const int lessonId = 1;

            var teacher = DataTestFactory.GetCarTeacher(teacherId);
            var student1 = DataTestFactory.GetCarStudent(studentId1);
            var car = DataTestFactory.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car, student1));

            // Act            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(new RemoveStudentFromLesson_Command(lessonId, student1.Id)));

            // Assert
            Assert.Equal("Il n'est pas possible de se désincrire moins de 24h avant le début du cours", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotRemoveInvalidUserFromLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid invalidStudentId = new Guid("00000000-0000-0000-0000-000000000004");
            DateTime lessonStart = _clock.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
            const int lessonId = 1;

            var teacher = DataTestFactory.GetCarTeacher(teacherId);
            var student1 = DataTestFactory.GetCarStudent(studentId1);
            var car = DataTestFactory.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car, student1));

            // Act            
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(new RemoveStudentFromLesson_Command(lessonId, invalidStudentId)));

            // Assert
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotRemoveStudentFromInvalidLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            DateTime lessonStart = _clock.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
            const int lessonId = 1;
            const int invalidLessonId = 2;

            var teacher = DataTestFactory.GetCarTeacher(teacherId);
            var student1 = DataTestFactory.GetCarStudent(studentId1);
            var car = DataTestFactory.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car, student1));

            // Act            
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(new RemoveStudentFromLesson_Command(invalidLessonId, student1.Id)));

            // Assert
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }
    }
}
