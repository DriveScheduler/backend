using Application.Abstractions;
using Application.UseCases.Lessons.Commands;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.TestData;

namespace UseCases.Schedule
{
    public class ScheduleAddStudentToWaitingList : IClassFixture<SetupDependencies>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IMediator _mediator;
        private readonly ISystemClock _clock;

        public ScheduleAddStudentToWaitingList(SetupDependencies fixture)
        {
            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();

            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }

        [Fact]
        public async Task ScheduleShould_AddStudentToWaitingList_WhenLessonIsFull()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000003");
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1));

            // Act
            await _mediator.Send(new AddStudentToWaitingList_Command(lessonId, student2.Id));
            Lesson lesson = _lessonRepository.GetById(lessonId)!;

            // Assert
            Assert.NotNull(lesson.Student);
            Assert.Single(lesson.WaitingList);
            Assert.Contains(student2, lesson.WaitingList);
        }

        [Fact]
        public async Task ScheduleShould_NotAddStudentToWaitingList_WhenLessonIsNotFull()
        {
            // Arrange
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000002");
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car));

            // Act
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(new AddStudentToWaitingList_Command(lessonId, student1.Id)));

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

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User student3 = DataSet.GetCarStudent(studentId3);
            User student4 = DataSet.GetCarStudent(studentId4);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(student3);
            _userRepository.Insert(student4);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1));

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
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000005");
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(teacher2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1));

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
            int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetMotorcycleStudent(studentId2);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1));

            // Act
            var command = new AddStudentToWaitingList_Command(lessonId, studentId2);

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
            Guid invalidStudentId = new Guid("00000000-0000-0000-0000-000000000004");
            int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", _clock.Now, 30, teacher, LicenceType.Car, car, student1));

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
            DateTime lessonStart = _clock.Now.AddSeconds(-1);
            const int lessonId = 1;

            User teacher = DataSet.GetCarTeacher(teacherId);
            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            Vehicle car = DataSet.GetCar(1);
            _userRepository.Insert(teacher);
            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(new Lesson(lessonId, "Cours 1", lessonStart, 30, teacher, LicenceType.Car, car, student1));

            // Act
            var command = new AddStudentToLesson_Command(lessonId, studentId2);

            // Assert            
            LessonValidationException exc = await Assert.ThrowsAsync<LessonValidationException>(() => _mediator.Send(command));
            Assert.Equal("Le cours est déjà passé", exc.Message);
        }
    }
}
