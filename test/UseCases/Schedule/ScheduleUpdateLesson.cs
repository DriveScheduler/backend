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
    public class ScheduleUpdateLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;

        public ScheduleUpdateLesson(SetupDependencies fixture)
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
        public async void ScheduleShould_UpdateLesson()
        {
            // Arrange          
            const string updatedLessonName = "Cours 2";
            DateTime updatedStartTime = _clock.Now.AddDays(1);
            const int updatedDuration = 30;


            Guid carTeacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid carTeacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;


            User carTeacher = DataSet.GetCarTeacher(carTeacherId);
            User carTeacher2 = DataSet.GetCarTeacher(carTeacherId2);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(carTeacher);
            _userRepository.Insert(carTeacher2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, carTeacher, LicenceType.Car, car));

            // Act
            var command = new UpdateLesson_Command(lessonId, updatedLessonName, updatedStartTime, updatedDuration, carTeacherId2);
            await _mediator.Send(command);
            Lesson? updatedLesson = _lessonRepository.GetById(lessonId);

            // Assert            
            Assert.NotNull(updatedLesson);
            Assert.Equal(updatedLessonName, updatedLesson.Name);
            Assert.Equal(updatedStartTime, updatedLesson.Start);
            Assert.Equal(updatedDuration, updatedLesson.Duration.Value);
            Assert.Equal(carTeacherId2, updatedLesson.Teacher.Id);
            Assert.Equal(car.RegistrationNumber, updatedLesson.Vehicle.RegistrationNumber);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithName()
        {
            // Arrange            
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new UpdateLesson_Command(lessonId, "", _clock.Now, 30, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le nom du cours est obligatoire", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithValidDate()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int lessonId = 1;


            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now.AddMinutes(-1), 30, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le date et l'heure du cours ne peuvent pas être inférieur à maintenant", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithExistingTeacher()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid invalidTeacherId = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;


            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, invalidTeacherId);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithValidTeacherForLessonType_WhenChangingTeacher()
        {
            // Arrange

            Guid carTeacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid truckTeacherId = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;


            User carTeacher = DataSet.GetCarTeacher(carTeacherId);
            User truckTeacher = DataSet.GetTruckTeacher(truckTeacherId);
            Vehicle car = DataSet.GetCar(1);
            Vehicle car2 = DataSet.GetCar(2);
            _userRepository.Insert(carTeacher);
            _userRepository.Insert(truckTeacher);
            _vehicleRepository.Insert(car);
            _vehicleRepository.Insert(car2);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, carTeacher, LicenceType.Car, car));

            // Act
            var command = new CreateLesson_Command("Cours 1", _clock.Now, 30, truckTeacherId, LicenceType.Car);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le moniteur doit pouvoir assurer ce type de cours", exc.Message);
        }


        [Fact]
        public async void ScheduleShould_UpdateLesson_WithAvailableTeacher()
        {
            // Arrange
            Guid teacherId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;

            User teacher1 = DataSet.GetCarTeacher(teacherId1);
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            Vehicle car1 = DataSet.GetCar(1);
            Vehicle car2 = DataSet.GetCar(2);

            _userRepository.Insert(teacher1);
            _userRepository.Insert(teacher2);
            _vehicleRepository.Insert(car1);
            _vehicleRepository.Insert(car2);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher1, LicenceType.Car, car1));
            _lessonRepository.Insert(new Lesson(2, "Cours 2", _clock.Now.AddMinutes(15), 30, teacher2, LicenceType.Car, car2));

            // Act
            var comamnd = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now.AddMinutes(20), 30, teacherId2);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(comamnd));
            Assert.Equal("Le moniteur n'est pas disponible pour cette plage horaire", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_MustBeATeacher()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student = DataSet.GetCarStudent(studentId);
            Vehicle car = DataSet.GetCar(1);

            _userRepository.Insert(teacher);
            _userRepository.Insert(student);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));


            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, studentId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("La personne en charge du cours doit être un moniteur", exc.Message);
        }

        [Theory]
        [InlineData(29)]
        [InlineData(121)]
        public async void ScheduleShould_UpdateLesson_WithValidDuration(int invalidDuration)
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, invalidDuration, teacherId);

            // Assert
            await Assert.ThrowsAsync<LessonDurationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_KeepVehicleIfAvailableAfterUpdate()
        {
            User teacher = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000001"));
            Vehicle car1 = DataSet.GetCar(1);
            Vehicle car2 = DataSet.GetCar(2);
            Lesson lesson = new Lesson(1, "Cours1", _clock.Now, 30, teacher, LicenceType.Car, car1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert([car1, car2]);
            _lessonRepository.Insert(lesson);

            var command = new UpdateLesson_Command(1, "Cours1", _clock.Now.AddMinutes(30), 30, teacher.Id);
            await _mediator.Send(command);

            Lesson updatedLesson = _lessonRepository.GetById(lesson.Id);
            Assert.Equal(car1.RegistrationNumber, updatedLesson.Vehicle.RegistrationNumber);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_ChangeVehicleIfCurrentIsntAvailableAfterUpdate()
        {
            User teacher1 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000001"));
            User teacher2 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000002"));
            Vehicle car1 = DataSet.GetCar(1);
            Vehicle car2 = DataSet.GetCar(2);
            Lesson lesson = new Lesson(1, "Cours1", _clock.Now, 30, teacher1, LicenceType.Car, car1);
            Lesson lessonWithVehicle1 = new Lesson(2, "Cours2", _clock.Now.AddMinutes(20), 30, teacher2, LicenceType.Car, car1);
            _userRepository.Insert([teacher1, teacher2]);
            _vehicleRepository.Insert([car1, car2]);
            _lessonRepository.Insert([lesson, lessonWithVehicle1]);

            var command = new UpdateLesson_Command(1, "Cours1", _clock.Now.AddMinutes(30), 30, teacher1.Id);
            await _mediator.Send(command);

            Lesson updatedLesson = _lessonRepository.GetById(lesson.Id);
            Assert.Equal(car2.RegistrationNumber, updatedLesson.Vehicle.RegistrationNumber);
        }

        [Fact]
        public async void ScheduleShould_NotUpdateLesson_IfNoVehiclesAreAvailableAfterUpdate()
        {
            User teacher1 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000001"));
            User teacher2 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000002"));
            User teacher3 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000003"));
            Vehicle car1 = DataSet.GetCar(1);
            Vehicle car2 = DataSet.GetCar(2);
            Lesson lesson = new Lesson(1, "Cours1", _clock.Now, 30, teacher1, LicenceType.Car, car1);
            Lesson lessonWithVehicle1 = new Lesson(2, "Cours2", _clock.Now.AddMinutes(20), 30, teacher2, LicenceType.Car, car1);
            Lesson lessonWithVehicle2 = new Lesson(3, "Cours3", _clock.Now.AddMinutes(10), 60, teacher3, LicenceType.Car, car2);
            _userRepository.Insert([teacher1, teacher2, teacher3]);
            _vehicleRepository.Insert([car1, car2]);
            _lessonRepository.Insert([lesson, lessonWithVehicle1, lessonWithVehicle2]);


            var command = new UpdateLesson_Command(1, "Cours1", _clock.Now.AddMinutes(30), 30, teacher1.Id);
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));

            Assert.Equal("Aucun vehicule disponibe pour valider ce cours", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotUpdateLesson_WhenLessonIsPassed()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            DateTime lessonStart = _clock.Now.AddSeconds(-1);
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, teacherId);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le cours est déjà passé", exc.Message);
        }

        [Fact]
        public async void CanNotUpdateInvalidLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int invalidLessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);

            // Act
            var command = new UpdateLesson_Command(invalidLessonId, "Cours 1", _clock.Now, 30, teacherId);

            // Assert
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }
    }
}
