using Application.UseCases.Lessons.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Exceptions.Vehicles;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleCreateLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;
        private readonly IDatabase _database;

        public ScheduleCreateLesson(SetupDependencies fixture)
        {
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
            _database = fixture.ServiceProvider.GetRequiredService<IDatabase>();
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
            const LicenceType licenceType = LicenceType.Car;
            const int maxStudent = 5;

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int vehicleId = 1;

            _database.Users.Add(DataSet.GetCarTeacher(teacherId));
            _database.Vehicles.Add(DataSet.GetCar(vehicleId));
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId, licenceType, vehicleId, maxStudent);
            int lessonId = await _mediator.Send(command);
            Lesson? lesson = _database.Lessons.Include(l => l.Teacher).Include(l => l.Vehicle).FirstOrDefault(l => l.Id == lessonId);

            // Assert
            Assert.NotEqual(default, lessonId);
            Assert.NotNull(lesson);
            Assert.Equal(lessonName, lesson.Name);
            Assert.Equal(dateTime, lesson.Start);
            Assert.Equal(duration, lesson.Duration);
            Assert.Equal(licenceType, lesson.Type);
            Assert.Equal(teacherId, lesson.Teacher.Id);
            Assert.Equal(vehicleId, lesson.Vehicle.Id);
            Assert.Equal(maxStudent, lesson.MaxStudent);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithName()
        {
            // Arrange
            const string lessonName = "";
            DateTime dateTime = _clock.Now;
            const int duration = 30;
            const LicenceType licenceType = LicenceType.Car;
            const int maxStudent = 5;

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");

            Vehicle car = DataSet.GetCar(1);

            _database.Users.Add(DataSet.GetCarTeacher(teacherId));
            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId, licenceType, car.Id, maxStudent);

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
            const LicenceType licenceType = LicenceType.Car;
            const int maxStudent = 5;

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Vehicle car = DataSet.GetCar(1);

            _database.Users.Add(DataSet.GetCarTeacher(teacherId));
            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId, licenceType, car.Id, maxStudent);

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
            const LicenceType licenceType = LicenceType.Car;
            const int maxStudent = 5;

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Vehicle car = DataSet.GetCar(1);

            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId, licenceType, car.Id, maxStudent);

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
            const LicenceType licenceType = LicenceType.Car;
            const int maxStudent = 5;

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Vehicle car = DataSet.GetCar(1);

            _database.Users.Add(DataSet.GetTruckTeacher(teacherId));
            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command(lessonName, dateTime, duration, teacherId, licenceType, car.Id, maxStudent);

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

            _database.Users.Add(teacher);
            _database.Vehicles.Add(vehicle);
            _database.Lessons.Add(new Lesson() { Id = 1, Name = "Cours 1", Start = _clock.Now, Duration = 30, Teacher = teacher, Type = LicenceType.Car, Vehicle = vehicle });
            await _database.SaveChangesAsync();

            // Act
            var createLessonWithSameVehicleAtSameRangeTimeCommand = new CreateLesson_Command("Cours 2", _clock.Now.AddMinutes(20), 30, teacherId, LicenceType.Car, vehicle.Id, 5);

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

            _database.Users.Add(DataSet.GetCarStudent(studentId));
            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();


            // Act
            var createLessonWithSameVehicleAtSameTimeCommand = new CreateLesson_Command("Cours 1", _clock.Now, 30, studentId, LicenceType.Car, car.Id, 5);

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
            const LicenceType licenceType = LicenceType.Car;
            const int maxStudent = 5;

            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Vehicle car = DataSet.GetCar(1);

            _database.Users.Add(DataSet.GetCarTeacher(teacherId));
            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command(LessonName, dateTime, invalidDuration, teacherId, licenceType, car.Id, maxStudent);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("La durée du cours n'est pas valide (elle doit être comprise entre 30min et 2h)", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithExistingVehicle()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");

            _database.Users.Add(DataSet.GetCarTeacher(teacherId));
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command("Cours 2", _clock.Now.AddMinutes(20), 30, teacherId, LicenceType.Car, 1, 5);

            // Assert
            VehicleNotFoundException exc = await Assert.ThrowsAsync<VehicleNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("Le vehicule n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_CreateLesson_WithAvailableVehicle()
        {
            // Arrange
            Guid teacherId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            int vehicleId = 1;

            User teacher1 = DataSet.GetCarTeacher(teacherId1);
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            Vehicle car = DataSet.GetCar(1);

            _database.Users.Add(teacher1);
            _database.Users.Add(teacher2);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Name = "Cours 1", Start = _clock.Now, Duration = 30, Teacher = teacher1, Type = LicenceType.Car, Vehicle = car });
            await _database.SaveChangesAsync();

            // Act
            var createLessonWithSameVehicleAtSameTimeCommand = new CreateLesson_Command("Cours 2", _clock.Now.AddMinutes(20), 30, teacher2.Id, LicenceType.Car, car.Id, 5);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(createLessonWithSameVehicleAtSameTimeCommand));
            Assert.Equal("Le vehicule demandé n'est pas disponible pour cette plage horaire", exc.Message);
        }


        [Fact]
        public async void ScheduleShould_CreateLesson_WithValidVehiculForLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Vehicle car = DataSet.GetCar(1);

            _database.Users.Add(DataSet.GetMotorcycleTeacher(teacherId));
            _database.Vehicles.Add(car);
            await _database.SaveChangesAsync();

            // Act
            var createLessonCommand = new CreateLesson_Command("Cours 1", _clock.Now, 30, teacherId, LicenceType.Motorcycle, car.Id, 5);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(createLessonCommand));
            Assert.Equal("Le type de vehicule ne correspond pas au type de cours", exc.Message);
        }

    }
}
