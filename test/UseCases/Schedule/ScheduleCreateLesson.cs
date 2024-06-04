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
    public class ScheduleCreateLesson
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;
        
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;

        public ScheduleCreateLesson()
        {
            SetupDependencies fixture = new SetupDependencies();
            fixture.BuildDefault();

            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }      

        [Fact]
        public async void ScheduleShould_CreateLesson()
        {
            // Arrange
            const string lessonName = "Cours 1";
            DateTime dateTime = _clock.Now.AddDays(1);
            const int duration = 30;            

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int vehicleId = 1;

            var teacher = DataTestFactory.GetCarTeacher(teacherId);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(DataTestFactory.GetCar(1));

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId);
            int lessonId = await _mediator.Send(command);
            Lesson? lesson = _lessonRepository.GetById(lessonId);

            // Assert
            Assert.NotEqual(default, lessonId);
            Assert.NotNull(lesson);
            Assert.Equal(lessonName, lesson.Name);
            Assert.Equal(dateTime, lesson.Start);
            Assert.Equal(duration, lesson.Duration.Value);
            Assert.Equal(teacher.LicenceType, lesson.Type);
            Assert.Equal(teacherId, lesson.Teacher.Id);
            Assert.Equal(vehicleId, lesson.Vehicle.Id);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithName()
        {
            // Arrange
            const string lessonName = "";
            DateTime dateTime = _clock.Now;
            const int duration = 30;            

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");

            var car = DataTestFactory.GetCar(1);

            _userRepository.Insert(DataTestFactory.GetCarTeacher(teacherId));
            _vehicleRepository.Insert(car);

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le nom du cours est obligatoire", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithValidDate()
        {
            // Arrange
            const string lessonName = "Cours 1";
            DateTime dateTime = _clock.Now.AddDays(-1);
            const int duration = 30;            

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            var car = DataTestFactory.GetCar(1);

            _userRepository.Insert(DataTestFactory.GetCarTeacher(teacherId));
            _vehicleRepository.Insert(car);

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithExistingTeacher()
        {
            // Arrange
            const string lessonName = "Cours 1";
            DateTime dateTime = _clock.Now.AddDays(-1);
            const int duration = 30;          

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            var car = DataTestFactory.GetCar(1);

            _vehicleRepository.Insert(car);

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }    


        [Fact]
        public async void ScheduleShould_CreateLesson_WithAvailableTeacher()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");

            var teacher = DataTestFactory.GetCarTeacher(teacherId);
            var vehicle = DataTestFactory.GetCar(1);
            var vehicle2 = DataTestFactory.GetCar(2);

            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(vehicle);
            _vehicleRepository.Insert(vehicle2);
            _lessonRepository.Insert(new Lesson(1, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, vehicle));

            // Act
            var createLessonWithSameVehicleAtSameRangeTimeCommand = new CreateLesson_Command("Cours 2", _clock.Now.AddMinutes(20), 30, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(createLessonWithSameVehicleAtSameRangeTimeCommand));
            Assert.Equal("Le moniteur n'est pas disponible pour cette plage horaire", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_MustBeATeacher()
        {
            // Arrange
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000001");
            var car = DataTestFactory.GetCar(1);

            _userRepository.Insert(DataTestFactory.GetCarStudent(studentId));
            _vehicleRepository.Insert(car);

            // Act
            var createLessonWithSameVehicleAtSameTimeCommand = new CreateLesson_Command("Cours 1", _clock.Now, 30, studentId);

            // Assert
            UserIsNotATeacherException exc = await Assert.ThrowsAsync<UserIsNotATeacherException>(() => _mediator.Send(createLessonWithSameVehicleAtSameTimeCommand));            
        }

        [Theory]
        [InlineData(29)]
        [InlineData(121)]
        public async void ScheduleShould_CreateLesson_WithValidDuration(int invalidDuration)
        {
            // Arrange
            const string LessonName = "Cours 1";
            DateTime dateTime = _clock.Now;            

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            var car = DataTestFactory.GetCar(1);

            _userRepository.Insert(DataTestFactory.GetCarTeacher(teacherId));
            _vehicleRepository.Insert(car);

            // Act
            var command = new CreateLesson_Command(LessonName, dateTime, invalidDuration, teacherId);

            // Assert
            await Assert.ThrowsAsync<LessonDurationException>(() => _mediator.Send(command));            
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithExistingVehicle()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");

            _userRepository.Insert(DataTestFactory.GetCarTeacher(teacherId));

            // Act
            var command = new CreateLesson_Command("Cours 2", _clock.Now.AddMinutes(20), 30, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Aucun vehicule disponibe pour valider ce cours", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithAvailableVehicle()
        {
            // Arrange
            Guid teacherId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid teacherId3 = new Guid("00000000-0000-0000-0000-000000000003");

            var teacher1 = DataTestFactory.GetCarTeacher(teacherId1);
            var teacher2 = DataTestFactory.GetCarTeacher(teacherId2);
            var teacher3 = DataTestFactory.GetCarTeacher(teacherId3);
            var car = DataTestFactory.GetCar(1);
            var car2 = DataTestFactory.GetCar(2);
            var car3 = DataTestFactory.GetCar(3);

            _userRepository.Insert(teacher1);
            _userRepository.Insert(teacher2);
            _userRepository.Insert(teacher3);
            _vehicleRepository.Insert(car);
            _vehicleRepository.Insert(car2);
            _vehicleRepository.Insert(car3);
            _lessonRepository.Insert(new Lesson("Cours 1", _clock.Now, 30, teacher1, LicenceType.Car, car));
            _lessonRepository.Insert(new Lesson("Cours 2", _clock.Now.AddMinutes(30), 30, teacher2, LicenceType.Car, car2));

            // Act
            var command = new CreateLesson_Command("Cours 3", _clock.Now.AddMinutes(20), 30, teacher3.Id);
            int lessonId = await _mediator.Send(command);
            Lesson? lesson = _lessonRepository.GetById(lessonId);

            // Assert
            Assert.NotNull(lesson);
            Assert.Equal(car3, lesson.Vehicle);
        }
    }
}
