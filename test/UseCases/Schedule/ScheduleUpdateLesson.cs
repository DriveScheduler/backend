using Application.UseCases.Lessons.Commands;
using Application.UseCases.Users.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Exceptions.Vehicles;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UseCases.Schedule
{
    public class ScheduleUpdateLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly IDatabase _database;
        private readonly ISystemClock _clock;

        public ScheduleUpdateLesson(SetupDependencies dependencies)
        {
            _mediator = dependencies.ServiceProvider.GetRequiredService<IMediator>();
            _database = dependencies.ServiceProvider.GetRequiredService<IDatabase>();
            _clock = dependencies.ServiceProvider.GetRequiredService<ISystemClock>();
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
            const LicenceType updatedLicenceType = LicenceType.Truck;
            const int updatedMaxStudent = 5;


            Guid carTeacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid truckTeacherId = new Guid("00000000-0000-0000-0000-000000000002");
            const int carVehicleId = 1;
            const int truckVehicleId = 2;
            const int lessonId = 1;


            Teacher carTeacher = new Teacher() { Id = carTeacherId, FirstName = "Car", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Teacher truckTeacher = new Teacher() { Id = truckTeacherId, FirstName = "Truck", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Truck };
            Vehicle car = new Vehicle() { Id = carVehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            Vehicle truck = new Vehicle() { Id = truckVehicleId, Name = "Camion", Type = LicenceType.Truck };
            _database.Teachers.Add(carTeacher);
            _database.Teachers.Add(truckTeacher);
            _database.Vehicles.Add(car);
            _database.Vehicles.Add(truck);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = carTeacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, updatedLessonName, updatedStartTime, updatedDuration, truckTeacherId, updatedLicenceType, truckVehicleId, updatedMaxStudent);
            await _mediator.Send(command);
            Lesson? updatedLesson = _database.Lessons.Include(l => l.Teacher).Include(l => l.Vehicle).FirstOrDefault(l => l.Id == lessonId);

            // Assert            
            Assert.NotNull(updatedLesson);
            Assert.Equal(updatedLessonName, updatedLesson.Name);
            Assert.Equal(updatedStartTime, updatedLesson.Start);
            Assert.Equal(updatedDuration, updatedLesson.Duration);
            Assert.Equal(truckTeacherId, updatedLesson.Teacher.Id);
            Assert.Equal(updatedLicenceType, updatedLesson.Type);
            Assert.Equal(truckVehicleId, updatedLesson.Vehicle.Id);
            Assert.Equal(updatedMaxStudent, updatedLesson.MaxStudent);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithName()
        {
            // Arrange            
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");            
            const int vehicleId = 1;            
            const int lessonId = 1;


            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };            
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };            
            _database.Teachers.Add(teacher);            
            _database.Vehicles.Add(car);            
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, "", _clock.Now, 30, teacherId, LicenceType.Car, vehicleId, 5);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le nom du cours est obligatoire", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithValidDate()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int vehicleId = 1;
            const int lessonId = 1;


            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now.AddMinutes(-1), 30, teacherId, LicenceType.Car, vehicleId, 5);

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
            const int vehicleId = 1;
            const int lessonId = 1;


            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, invalidTeacherId, LicenceType.Car, vehicleId, 5);

            // Assert
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithValidTeacherForLessonType_WhenChangingLessonType()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");            
            const int vehicleId = 1;
            const int lessonId = 1;

            const LicenceType invalidLicenceType = LicenceType.Motorcycle;


            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command("Cours 1", _clock.Now, 30, teacherId, invalidLicenceType, vehicleId, 5);

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
            const int vehicleId = 1;            
            const int lessonId = 1;


            Teacher carTeacher = new Teacher() { Id = carTeacherId, FirstName = "Car", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Teacher truckTeacher = new Teacher() { Id = truckTeacherId, FirstName = "Truck", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Truck };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };            
            _database.Teachers.Add(carTeacher);
            _database.Teachers.Add(truckTeacher);
            _database.Vehicles.Add(car);            
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = carTeacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new CreateLesson_Command("Cours 1", _clock.Now, 30, truckTeacherId, LicenceType.Car, vehicleId, 5);

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

            const int vehicleId1 = 1;
            const int vehicleId2 = 2;

            const int lessonId = 1;

            Teacher teacher1 = new Teacher() { Id = teacherId1, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Teacher teacher2 = new Teacher() { Id = teacherId2, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle vehicle1 = new Vehicle() { Id = vehicleId1, Name = "Peugeot 208", Type = LicenceType.Car };
            Vehicle vehicle2 = new Vehicle() { Id = vehicleId2, Name = "Peugeot 208", Type = LicenceType.Car };

            _database.Teachers.Add(teacher1);
            _database.Teachers.Add(teacher2);
            _database.Vehicles.Add(vehicle1);
            _database.Vehicles.Add(vehicle2);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Start = _clock.Now, Duration = 30, Teacher = teacher1, Type = LicenceType.Car, Vehicle = vehicle1 });
            _database.Lessons.Add(new Lesson() { Id = 2, Name = "Cours 2", Start = _clock.Now.AddMinutes(15), Duration = 30, Teacher = teacher2, Type = LicenceType.Car, Vehicle = vehicle2 });
            await _database.SaveChangesAsync();

            // Act
            var comamnd = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now.AddMinutes(20), 30, teacherId2, LicenceType.Car, vehicleId1, 5);

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

            const int vehicleId = 1;            

            const int lessonId = 1;

            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Student student = new Student() { Id = studentId, FirstName = "Student", Name = "Student", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle vehicle = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };            

            _database.Teachers.Add(teacher);
            _database.Students.Add(student);
            _database.Vehicles.Add(vehicle);            
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Start = _clock.Now, Duration = 30, Teacher = teacher, Type = LicenceType.Car, Vehicle = vehicle });
            await _database.SaveChangesAsync();


            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, studentId, LicenceType.Car, vehicleId, 5);

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
            const int vehicleId = 1;
            const int lessonId = 1;

            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, invalidDuration, teacherId, LicenceType.Car, vehicleId, 5);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("La durée du cours n'est pas valide (elle doit être comprise entre 30min et 2h)", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithExistingVehicle()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int vehicleId = 1;
            const int lessonId = 1;

            const int invalidVehicleId = 2;

            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, teacherId, LicenceType.Car, invalidVehicleId, 5);

            // Assert
            VehicleNotFoundException exc = await Assert.ThrowsAsync<VehicleNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("Le vehicule n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_UpdateLesson_WithAvailableVehicle()
        {
            // Arrange
            Guid teacherId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000002");
            const int vehicleId1 = 1;
            const int vehicleId2 = 2;
            const int lessonId1 = 1;
            const int lessonId2 = 2;

            Teacher teacher1 = new Teacher() { Id = teacherId1, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Teacher teacher2 = new Teacher() { Id = teacherId2, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle vehicle1 = new Vehicle() { Id = vehicleId1, Name = "Peugeot 208", Type = LicenceType.Car };
            Vehicle vehicle2 = new Vehicle() { Id = vehicleId2, Name = "Peugeot 208", Type = LicenceType.Car };

            _database.Teachers.Add(teacher1);
            _database.Teachers.Add(teacher2);
            _database.Vehicles.Add(vehicle1);
            _database.Vehicles.Add(vehicle2);
            _database.Lessons.Add(new Lesson() { Id = lessonId1, Name = "Cours 1", Start = _clock.Now, Duration = 30, Teacher = teacher1, Type = LicenceType.Car, Vehicle = vehicle1 });
            _database.Lessons.Add(new Lesson() { Id = lessonId2, Name = "Cours 2", Start = _clock.Now, Duration = 30, Teacher = teacher2, Type = LicenceType.Car, Vehicle = vehicle2 });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId1, "Cours 1", _clock.Now, 30, teacherId1, LicenceType.Car, vehicleId2, 5);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le vehicule demandé n'est pas disponible pour cette plage horaire", exc.Message);
        }


        [Fact]
        public async void ScheduleShould_UpdateLesson_WithValidVehiculForLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int carId = 1;
            const int truckId = 2;
            const int lessonId = 1;            

            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = carId, Name = "Peugeot 208", Type = LicenceType.Car };
            Vehicle truck = new Vehicle() { Id = truckId, Name = "Camion", Type = LicenceType.Truck };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);
            _database.Vehicles.Add(truck);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Teacher = teacher, Vehicle = car, Type = LicenceType.Car });
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(lessonId, "Cours 1", _clock.Now, 30, teacherId, LicenceType.Car, truckId, 5);

            // Assert
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le type de vehicule ne correspond pas au type de cours", exc.Message);
        }

        [Fact]
        public async void CanNotUpdateInvalidLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            const int vehicleId = 1;            
            
            const int invalidLessonId = 1;            

            Teacher teacher = new Teacher() { Id = teacherId, FirstName = "Teacher", Name = "Teacher", Email = "a@mail", LicenceType = LicenceType.Car };
            Vehicle car = new Vehicle() { Id = vehicleId, Name = "Peugeot 208", Type = LicenceType.Car };
            _database.Teachers.Add(teacher);
            _database.Vehicles.Add(car);            
            await _database.SaveChangesAsync();

            // Act
            var command = new UpdateLesson_Command(invalidLessonId, "Cours 1", _clock.Now, 30, teacherId, LicenceType.Car, vehicleId, 5);

            // Assert
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }
    }
}
