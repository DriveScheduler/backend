using Application.Abstractions;
using Application.UseCases.Lessons.Commands;

using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleRemoveStudentFromLesson : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly IDatabase _database;
        private readonly ISystemClock _clock;        

        public ScheduleRemoveStudentFromLesson(SetupDependencies setupDependencies)
        {
            _mediator = setupDependencies.ServiceProvider.GetRequiredService<IMediator>();
            _database = setupDependencies.ServiceProvider.GetRequiredService<IDatabase>();
            _clock = setupDependencies.ServiceProvider.GetRequiredService<ISystemClock>();            
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void ScheduleShould_RemoveStudentFromLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");             
            DateTime lessonStart = _clock.Now.AddHours(24);
            const int lessonId = 1;            

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);                      
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);                    
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1 });
            await _database.SaveChangesAsync();

            // Act
            await _mediator.Send(new RemoveStudentFromLesson_Command(lessonId, student1.Id));
            Lesson lesson = _database.Lessons.Find(lessonId)!;

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);        
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);    
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1 });
            await _database.SaveChangesAsync();

            // Act            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>( () => _mediator.Send(new RemoveStudentFromLesson_Command(lessonId, student1.Id)));

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);                    
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);            
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1 });
            await _database.SaveChangesAsync();

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);            
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);            
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1 });
            await _database.SaveChangesAsync();

            // Act            
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(new RemoveStudentFromLesson_Command(invalidLessonId, student1.Id)));

            // Assert
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }
    }
}
