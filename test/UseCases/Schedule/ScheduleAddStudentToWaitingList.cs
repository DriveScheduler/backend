using Application.UseCases.Lessons.Commands;

using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleAddStudentToWaitingList : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly IDatabase _database;
        private readonly ISystemClock _clock;

        public ScheduleAddStudentToWaitingList(SetupDependencies setupDependencies)
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
        public async Task ScheduleShould_AddStudentToWaitingList_WhenLessonIsFull()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");
            Guid studentId4 = new Guid("00000000-0000-0000-0000-000000000005");
            const int lessonId = 1;
            const int maxStudent = 3;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User student3 = DataSet.GetCarStudent(studentId3);
            User student4 = DataSet.GetCarStudent(studentId4);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);
            _database.Users.Add(student4);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = maxStudent, Students = [student1, student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            await _mediator.Send(new AddStudentToWaitingList_Command(lessonId, student4.Id));
            Lesson lesson = _database.Lessons.Find(lessonId)!;

            // Assert
            Assert.Equal(3, lesson.Students.Count);
            Assert.Single(lesson.WaitingList);
            Assert.Contains(student4, lesson.WaitingList);
        }

        [Fact]
        public async Task ScheduleShould_NotAddStudentToWaitingList_WhenLessonIsNotFull()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");
            Guid studentId4 = new Guid("00000000-0000-0000-0000-000000000005");
            const int lessonId = 1;
            const int maxStudent = 4;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User student3 = DataSet.GetCarStudent(studentId3);
            User student4 = DataSet.GetCarStudent(studentId4);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);
            _database.Users.Add(student4);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = maxStudent, Students = [student1, student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(new AddStudentToWaitingList_Command(lessonId, student4.Id)));

            // Assert
            Assert.Equal("Le cours n'est pas complet", exc.Message);
        }

        [Fact]
        public async Task ScheduleShould_NotAddStudentToWaitingList_WhenLessonNotExists()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");
            Guid studentId4 = new Guid("00000000-0000-0000-0000-000000000005");
            const int lessonId = 1;
            const int invalidLessonId = 2;
            const int maxStudent = 3;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User student3 = DataSet.GetCarStudent(studentId3);
            User student4 = DataSet.GetCarStudent(studentId4);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);
            _database.Users.Add(student4);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = maxStudent, Students = [student1, student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(new AddStudentToWaitingList_Command(invalidLessonId, student4.Id)));

            // Assert
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }

        [Fact]
        public async Task ScheduleShould_NotAddTeacherToWaitingList()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000005");
            const int lessonId = 1;
            const int maxStudent = 3;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User student3 = DataSet.GetCarStudent(studentId3);
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);
            _database.Users.Add(teacher2);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = maxStudent, Students = [student1, student2, student3] });
            await _database.SaveChangesAsync();

            // Act
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(new AddStudentToWaitingList_Command(lessonId, teacher2.Id)));

            // Assert
            Assert.Equal("L'utilisateur doit être un élève pour s'incrire à la file d'attente du cours", exc.Message);
        }

        [Fact]
        public async void SheduleShould_NotAddSameStudentToSameWaitingList()
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
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = 2, Students = [student1, student2], WaitingList = [student3] });
            await _database.SaveChangesAsync();

            // Act
            var command = new AddStudentToWaitingList_Command(lessonId, studentId3);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur est déjà dans la liste d'attente", exc.Message);
        }

        [Fact]
        public async void SheduleShould_NotAddStudentToWaitingList_WhenLicenceTypeNotMatch()
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
            User student3 = DataSet.GetMotorcycleStudent(studentId3);
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);
            _database.Users.Add(student3);
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = 2, Students = [student1, student2] });
            await _database.SaveChangesAsync();

            // Act
            var command = new AddStudentToWaitingList_Command(lessonId, studentId3);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le permis de l'utilisateur ne correspond pas au type de cours", exc.Message);
        }

        [Fact]
        public async void SheduleShould_NotAddInvalidStudentToWaitingList()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid invalidStudentId = new Guid("00000000-0000-0000-0000-000000000004");
            int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);            
            Vehicle car = DataSet.GetCar(1);
            _database.Users.Add(teacher);
            _database.Users.Add(student1);
            _database.Users.Add(student2);            
            _database.Vehicles.Add(car);
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = _clock.Now, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = 2, Students = [student1, student2] });
            await _database.SaveChangesAsync();

            // Act
            var command = new AddStudentToWaitingList_Command(lessonId, invalidStudentId);

            // Assert            
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
            Assert.Equal("L'utilisateur n'existe pas", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotAddStudentToWaitingLesson_WhenLessonPassed()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid studentId3 = new Guid("00000000-0000-0000-0000-000000000004");            
            DateTime lessonStart = _clock.Now.AddSeconds(-1);
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
            _database.Lessons.Add(new Lesson() { Id = lessonId, Name = "Cours 1", Duration = 30, Start = lessonStart, Type = LicenceType.Car, Teacher = teacher, Vehicle = car, MaxStudent = 3, Students = [student1, student2] });
            await _database.SaveChangesAsync();

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId3);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le cours est déjà passé", exc.Message);
        }
    }
}
