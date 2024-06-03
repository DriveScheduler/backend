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

using UseCases.Fakes;
using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleRemoveStudentFromWaitingList : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;
        private readonly FakeEmailSender _emailSender;

        public ScheduleRemoveStudentFromWaitingList(SetupDependencies fixture)
        {
            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();

            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
            _emailSender = (FakeEmailSender)fixture.ServiceProvider.GetRequiredService<IEmailSender>();
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

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student1 = DataSet.GetCarStudent(studentId1);
            var student2 = DataSet.GetStudent(studentId2, email: studentEmails[0]);
            var student3 = DataSet.GetStudent(studentId3, email: studentEmails[1]);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(student3);
            _vehicleRepository.Insert(car);
            Lesson lesson = new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car, student1);
            lesson.AddStudentToWaitingList(student2);
            lesson.AddStudentToWaitingList(student3);
            _lessonRepository.Insert(lesson);

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

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student1 = DataSet.GetCarStudent(studentId1);
            var student2 = DataSet.GetCarStudent(studentId2);
            var student3 = DataSet.GetCarStudent(studentId3);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(student3);
            _vehicleRepository.Insert(car);
            Lesson lesson = new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1);
            lesson.AddStudentToWaitingList(student2);
            lesson.AddStudentToWaitingList(student3);
            _lessonRepository.Insert(lesson);

            // Act
            await _mediator.Send(new RemoveStudentFromWaitingList_Command(lessonId, student3.Id));
            lesson = _lessonRepository.GetById(lessonId);

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

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student1 = DataSet.GetCarStudent(studentId1);
            var student2 = DataSet.GetCarStudent(studentId2);
            var student3 = DataSet.GetCarStudent(studentId3);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(student3);
            _vehicleRepository.Insert(car);
            Lesson lesson = new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1);
            lesson.AddStudentToWaitingList(student2);
            lesson.AddStudentToWaitingList(student3);
            _lessonRepository.Insert(lesson);

            // Act
            UserNotFoundException exc = await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(new RemoveStudentFromWaitingList_Command(lessonId, invalidUserId)));

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

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student1 = DataSet.GetCarStudent(studentId1);
            var student2 = DataSet.GetCarStudent(studentId2);
            var student3 = DataSet.GetCarStudent(studentId3);
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(student3);
            _vehicleRepository.Insert(car);
            Lesson lesson = new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1);
            lesson.AddStudentToWaitingList(student2);
            lesson.AddStudentToWaitingList(student3);
            _lessonRepository.Insert(lesson);

            // Act
            LessonNotFoundException exc = await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(new RemoveStudentFromWaitingList_Command(invalidLessonId, student3.Id)));

            // Assert
            Assert.Equal("Le cours n'existe pas", exc.Message);
        }
    }
}
