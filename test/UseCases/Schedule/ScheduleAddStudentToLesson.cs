using Application.UseCases.Lessons.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;


namespace UseCases.Schedule
{
    public class ScheduleAddStudentToLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly IDatabase _database;
        private readonly ISystemClock _clock;


        public ScheduleAddStudentToLesson(SetupDependencies dependencies)
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
        public async void SheduleShould_AddAStudentToLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student = DataSet.GetCarStudent(studentId);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car });
            await _database.SaveChangesAsync();

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId);
            await _mediator.Send(command);

            // Assert
            Lesson? lesson = _database.Lessons.Include(l => l.Student).FirstOrDefault(l => l.Id == lessonId);
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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User student3 = DataSet.GetCarStudent(studentId3);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1 });
            await _database.SaveChangesAsync();

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student = DataSet.GetMotorcycleStudent(studentId);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car });
            await _database.SaveChangesAsync();

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

            User teacher = DataSet.GetCarTeacher(teacherId);            
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);            
            _database.Users.Add(teacher2);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car });
            await _database.SaveChangesAsync();

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

            User teacher = DataSet.GetCarTeacher(teacherId);                       
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);            
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car });
            await _database.SaveChangesAsync();

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student = DataSet.GetCarStudent(studentId);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car });
            await _database.SaveChangesAsync();

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student = DataSet.GetCarStudent(studentId);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car });
            await _database.SaveChangesAsync();

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le cours est déjà passé", exc.Message);
        }
    }
}
