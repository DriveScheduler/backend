using Application.Abstractions;
using Application.UseCases.Lessons.Commands;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Exceptions.Vehicles;
using Domain.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleUpdateLesson : IClassFixture<SetupDependencies>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;


        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;

        public ScheduleUpdateLesson(SetupDependencies fixture)
        {
            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();

            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson()
        {
            // Arrange          
            const string updatedLessonName = "Cours 2";
            DateTime updatedStartTime = _clock.Now.AddDays(1);
            const int updatedDuration = 30;
            const LicenceType updatedLicenceType = LicenceType.Truck;


            Guid carTeacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid carTeacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;


            User carTeacher = DataSet.GetCarTeacher(carTeacherId);
            User carTeacher2 = DataSet.GetTruckTeacher(carTeacherId2);
            Vehicle car = DataSet.GetCar(1);
            Vehicle truck = DataSet.GetTruck(2);
            _userRepository.Insert(carTeacher);
            _userRepository.Insert(carTeacher2);
            _vehicleRepository.Insert(car);
            _vehicleRepository.Insert(truck);
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
            Assert.Equal(updatedLicenceType, updatedLesson.Type);
            Assert.Equal(truck.RegistrationNumber, updatedLesson.Vehicle.RegistrationNumber);
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
        public async void ScheduleShould_UpdateLesson_WithValidTeacherForLessonType_WhenChangingLessonType()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int lessonId = 1;

            const LicenceType invalidLicenceType = LicenceType.Motorcycle;


            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);
            Vehicle moto = DataSet.GetMotorcycle(2);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _vehicleRepository.Insert(moto);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            var command = new CreateLesson_Command("Cours 1", _clock.Now, 30, teacherId, invalidLicenceType);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le moniteur doit pouvoir assurer ce type de cours", exc.Message);
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
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("La durée du cours n'est pas valide (elle doit être comprise entre 30min et 2h)", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithAvailableVehicle_TOOOOOOOOOOOODDDDDDDDOOOOOOOOOO()
        {
            // TESTER LE FAIT DE CHANGER LA PLAGE HORAIRE DU COURS SUR UNE PLAGE AVEC AUCUN VEHICULE DE DISPONIBLE REMONTE UNE ERREUR
            //// Arrange
            //Guid teacherId1 = new Guid("00000000-0000-0000-0000-000000000001");
            //Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            //const int lessonId1 = 1;
            //const int lessonId2 = 2;

            //User teacher1 = DataSet.GetCarTeacher(teacherId1);
            //User teacher2 = DataSet.GetCarTeacher(teacherId2);
            //Vehicle car1 = DataSet.GetCar(1);
            //Vehicle car2 = DataSet.GetCar(2);

            //_userRepository.Insert(teacher1);
            //_userRepository.Insert(teacher2);
            //_vehicleRepository.Insert(car1);
            //_vehicleRepository.Insert(car2);
            //_lessonRepository.Insert(new Lesson() { Id = lessonId1, Name = "Cours 1", Start = _clock.Now, Duration = 30, Teacher = teacher1, Type = LicenceType.Car, Vehicle = car1 });
            //_lessonRepository.Insert(new Lesson() { Id = lessonId2, Name = "Cours 2", Start = _clock.Now, Duration = 30, Teacher = teacher2, Type = LicenceType.Car, Vehicle = car2 });
            //await _database.SaveChangesAsync();

            //// Act
            //var command = new UpdateLesson_Command(lessonId1, "Cours 1", _clock.Now, 30, teacherId1);

            //// Assert
            //LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            //Assert.Equal("Le vehicule demandé n'est pas disponible pour cette plage horaire", exc.Message);
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
