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
    public class ScheduleAddStudentToLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;


        public ScheduleAddStudentToLesson(SetupDependencies fixture)
        {
            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();

            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void SheduleShould_AddAStudentToLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student = DataSet.GetCarStudent(studentId);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId);
            await _mediator.Send(command);

            // Assert
            Lesson? lesson = _lessonRepository.GetById(lessonId);
            Assert.NotNull(lesson);
            Assert.Equal(student, lesson.Student);
        }

        [Fact]
        public async void ScheduleShould_NotAddStudentToFullLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student1 = DataSet.GetCarStudent(studentId1);
            var student2 = DataSet.GetCarStudent(studentId2);
            var student3 = DataSet.GetCarStudent(studentId3);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(student3);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId3);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le cours est complet", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotAddStudentToLesson_WhenLicenceTypeNotMatch()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student = DataSet.GetMotorcycleStudent(studentId);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le permis de l'utilisateur ne correspond pas au type de cours", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotAddTeacherToLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000003");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var teacher2 = DataSet.GetCarTeacher(teacherId2);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(teacher2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, teacherId2);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur doit être un élève pour s'incrire au cours", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotAddInvalidUserToLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid invalidUserId = new Guid("00000000-0000-0000-0000-000000000003");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, invalidUserId);

            // Assert            
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotAddStudentToInvalidLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;
            const int invalidLessonId = 2;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student = DataSet.GetCarStudent(studentId);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new AddStudentToLesson_Command(invalidLessonId, studentId);

            // Assert            
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotAddStudentToLesson_WhenLessonPassed()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            DateTime lessonStart = _clock.Now.AddSeconds(-1);
            const int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student = DataSet.GetCarStudent(studentId);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le cours est déjà passé", exc.Message);
        }        
    }
}
