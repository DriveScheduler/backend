using Application.UseCases.Lessons.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.Fakes;
using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleRemoveStudentFromWaitingList : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly IDatabase _database;
        private readonly ISystemClock _clock;
        private readonly FakeEmailSender _emailSender;

        public ScheduleRemoveStudentFromWaitingList(SetupDependencies setupDependencies)
        {
            _mediator = setupDependencies.ServiceProvider.GetRequiredService<IMediator>();
            _database = setupDependencies.ServiceProvider.GetRequiredService<IDatabase>();
            _clock = setupDependencies.ServiceProvider.GetRequiredService<ISystemClock>();
            _emailSender = (FakeEmailSender)setupDependencies.ServiceProvider.GetRequiredService<IEmailSender>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public async void ScheduleShould_RemoveStudentFromLesson_And_NotifyWaitingStudents()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");            
            DateTime lessonStart = _clock.Now.AddHours(24);
            List<string> studentEmails = ["student1@gmail.com", "student2@gmail.com"];
            
            const int lessonId = 1;            

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);            
            User student2 = DataSet.GetStudent(studentId2, email: studentEmails[0]);
            User student3 = DataSet.GetStudent(studentId3, email: studentEmails[1]);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);            
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1, WaitingList = [student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            await _mediator.Send(new RemoveStudentFromLesson_Command(lessonId, student1.Id));            

            // Assert
            List<string> emailsSentToAddress = _emailSender.EmailsSent.Select(s => s.To).ToList();
            Assert.Equal(2, emailsSentToAddress.Count);
            Assert.Contains(studentEmails[0], emailsSentToAddress);
            Assert.Contains(studentEmails[1], emailsSentToAddress);
        }


        [Fact]
        public async void ScheduleShould_RemoveStudentFromWaitingList()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");            
            
            const int lessonId = 1;
            
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
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1, WaitingList = [student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            await _mediator.Send(new RemoveStudentFromWaitingList_Command(lessonId, student3.Id));
            Lesson lesson = _database.Lessons.Find(lessonId)!;

            // Assert
            Assert.Single(lesson.WaitingList);
            Assert.DoesNotContain(student3, lesson.WaitingList);
        }

        [Fact]
        public async void ScheduleShould_NotRemoveInvalidUserFromWaitingList()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");           
            Guid invalidUserId = new Guid("00000000-0000-0000-0000-000000000007");

            const int lessonId = 1;
            
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
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1, WaitingList = [student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            UserNotFoundException exc = await Assert.ThrowsAsync< UserNotFoundException>( () => _mediator.Send(new RemoveStudentFromWaitingList_Command(lessonId, invalidUserId)));            

            // Assert
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);            
        }

        [Fact]
        public async void ScheduleShould_NotRemoveStudentFromInvalidLessonWaitingList()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");                       

            const int lessonId = 1;
            const int invalidLessonId = 2;            

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
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, Student = student1, WaitingList = [student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(new RemoveStudentFromWaitingList_Command(invalidLessonId, student3.Id)));

            // Assert
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }
    }
}
