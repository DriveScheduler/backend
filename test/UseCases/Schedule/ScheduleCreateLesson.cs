using Application.Abstractions;
using Application.UseCases.Lessons.Commands;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;
using Infrastructure.Persistence;

namespace UseCases.Schedule
{
    public class ScheduleCreateLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;

        public ScheduleCreateLesson(SetupDependencies fixture)
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
        public async void ScheduleShould_CreateLesson()
        {
            // Arrange
            const string lessonName = "Cours 1";
            DateTime dateTime = _clock.Now.AddDays(1);
            const int duration = 30;            

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int vehicleId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(DataSet.GetCar(1));

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

            Vehicle car = DataSet.GetCar(1);

            _userRepository.Insert(DataSet.GetCarTeacher(teacherId));
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
            Vehicle car = DataSet.GetCar(1);

            _userRepository.Insert(DataSet.GetCarTeacher(teacherId));
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
            Vehicle car = DataSet.GetCar(1);

            _vehicleRepository.Insert(car);

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithValidTeacherForLessonType()
        {
            // Arrange
            const string lessonName = "Cours 1";
            DateTime dateTime = _clock.Now;
            const int duration = 30;            

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Vehicle car = DataSet.GetCar(1);

            _userRepository.Insert(DataSet.GetTruckTeacher(teacherId));
            _vehicleRepository.Insert(car);

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le moniteur doit pouvoir assurer ce type de cours", exc.Message);
        }


        [Fact]
        public async void ScheduleShould_CreateLesson_WithAvailableTeacher()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");

            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle vehicle = DataSet.GetCar(1);
            Vehicle vehicle2 = DataSet.GetCar(2);

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
            Vehicle car = DataSet.GetCar(1);

            _userRepository.Insert(DataSet.GetCarStudent(studentId));
            _vehicleRepository.Insert(car);

            // Act
            var createLessonWithSameVehicleAtSameTimeCommand = new CreateLesson_Command("Cours 1", _clock.Now, 30, studentId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(createLessonWithSameVehicleAtSameTimeCommand));
            Assert.Equal("La personne en charge du cours doit être un moniteur", exc.Message);
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
            Vehicle car = DataSet.GetCar(1);

            _userRepository.Insert(DataSet.GetCarTeacher(teacherId));
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

            _userRepository.Insert(DataSet.GetCarTeacher(teacherId));

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

            User teacher1 = DataSet.GetCarTeacher(teacherId1);
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            User teacher3 = DataSet.GetCarTeacher(teacherId3);
            Vehicle car = DataSet.GetCar(1);
            Vehicle car2 = DataSet.GetCar(2);
            Vehicle car3 = DataSet.GetCar(3);

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
