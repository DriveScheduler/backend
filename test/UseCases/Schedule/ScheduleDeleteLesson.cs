using Application.Abstractions;
using Application.UseCases.Lessons.Commands;

using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.Fakes;
using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleDeleteLesson
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;
        
        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;
        private readonly FakeEmailSender _emailSender;

        public ScheduleDeleteLesson()
        {
            SetupDependencies fixture = new SetupDependencies();
            fixture
                .AddDefaultDependencies()
                .AddFakeEmailSender()
                .Build();

            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
            
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
            _emailSender = (FakeEmailSender)fixture.ServiceProvider.GetRequiredService<IEmailSender>();
        }
     

        [Fact]
        public async void ScheduleShould_DeleteLesson()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");            
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);            
            var car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);            
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act            
            await _mediator.Send(new DeleteLesson_Command(lessonId, teacherId));

            // Assert
            Assert.Throws<LessonNotFoundException>( () =>  _lessonRepository.GetById(lessonId));            
        }

        [Fact]
        public async void ScheduleShould_NotDeleteLesson_WhenTeacherIsntOwner()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid otherTeacherId = new Guid("00000000-0000-0000-0000-000000000002");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var otherTeacher = DataSet.GetCarTeacher(otherTeacherId);
            var car = DataSet.GetCar(1);
            _userRepository.Insert([teacher, otherTeacher]);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>( () => _mediator.Send(new DeleteLesson_Command(lessonId, otherTeacherId)));

            // Assert
            Assert.Equal("Vous n'êtes pas le professeur de ce cours", exc.Message);
        }

        [Fact]
        public async void ScheduleShould_NotDeleteLesson_WhenLessonIsntValid()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");            
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);            
            var car = DataSet.GetCar(1);
            _userRepository.Insert([teacher]);
            _vehicleRepository.Insert(car);            

            // Act            
            await Assert.ThrowsAsync<LessonNotFoundException>(() => _mediator.Send(new DeleteLesson_Command(lessonId, teacherId)));                        
        }

        [Fact]
        public async void ScheduleShould_DeleteLesson_ThenSendEmailToEveryStudents()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000002");
            Guid waitingStudentId1 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid waitingStudentId2 = new Guid("00000000-0000-0000-0000-000000000004");
            int lessonId = 1;

            var teacher = DataSet.GetCarTeacher(teacherId);
            var student = DataSet.GetStudent(studentId, type: LicenceType.Car, email: "student@gmail.com");
            var waitingStudent1 = DataSet.GetStudent(waitingStudentId1, type: LicenceType.Car, email: "waiting.student1@gmail.com");
            var waitingStudent2 = DataSet.GetStudent(waitingStudentId2, type: LicenceType.Car, email: "waiting.student2@gmail.com");
            var car = DataSet.GetCar(1);
            _userRepository.Insert([teacher, student, waitingStudent1, waitingStudent2]);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours", _clock.Now, 30, teacher, LicenceType.Car, car, student));

            await _mediator.Send(new AddStudentToWaitingList_Command(lessonId, waitingStudentId1));
            await _mediator.Send(new AddStudentToWaitingList_Command(lessonId, waitingStudentId2));

            // Act            
            await _mediator.Send(new DeleteLesson_Command(lessonId, teacherId));

            List<string> emailList = _emailSender.EmailsSent.Select(email => email.To).ToList();
            Assert.Equal(3, _emailSender.EmailsSent.Count);
            Assert.Contains(student.Email.Value, emailList);
            Assert.Contains(waitingStudent1.Email.Value, emailList);
            Assert.Contains(waitingStudent2.Email.Value, emailList);
            
        }
    }    
}
