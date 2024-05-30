using Application.Abstractions;
using Application.UseCases.Users.Queries;

using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.Fakes;
using UseCases.TestData;
using Infrastructure.Persistence;

namespace UseCases.Schedule
{
    public class ScheduleGetPlanning : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;
        private readonly FakeSystemClock _clock;

        public ScheduleGetPlanning(SetupDependencies fixture)
        {
            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();

            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _clock = (FakeSystemClock)fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }

        public void Dispose()
        {
            _database.Clear();
        }


        [Fact]
        public async void ScheduleShould_GetPlanningForAStudent()
        {
            _clock.Set(new DateTime(2024, 05, 15));

            // Arrange
            Guid studentId = new Guid("00000000-0000-0000-0000-000000000001");
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000002");

            User student = DataSet.GetCarStudent(studentId);
            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);

            DateTime rangeStart = new DateTime(2024, 05, 13);
            DateTime rangeEnd = new DateTime(2024, 05, 17);

            List<Lesson> inRangeLessons = new List<Lesson>()
            {
                new Lesson(1, "Lesson1", new DateTime(2024, 05, 13,  8, 0,0), 30, teacher, LicenceType.Car, car, student),
                new Lesson(2, "Lesson2", new DateTime(2024, 05, 17, 18, 30, 0), 30, teacher, LicenceType.Car, car, student),
            };
            List<Lesson> outOfRangeLessons = new List<Lesson>()
            {
                new Lesson(3, "Lesson3", new DateTime(2024, 05, 12, 17, 0, 0), 30, teacher, LicenceType.Car, car, student),
                new Lesson(4, "Lesson4", new DateTime(2024, 05, 18, 8, 0, 0), 30, teacher, LicenceType.Car, car, student),
            };

            _userRepository.Insert(student);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(inRangeLessons);
            _lessonRepository.Insert(outOfRangeLessons);

            // Act
            var query = new GetUserPlanning_Query(studentId, rangeStart, rangeEnd);
            List<Lesson> planning = await _mediator.Send(query);

            // Assert
            Assert.NotNull(planning);
            Assert.Equal(inRangeLessons, planning);
            Assert.DoesNotContain(outOfRangeLessons, l => planning.Contains(l));
        }

        [Fact]
        public async void ScheduleShould_GetPlanningForATeacher()
        {
            _clock.Set(new DateTime(2024, 05, 15));

            // Arrange
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000003");

            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);

            DateTime rangeStart = new DateTime(2024, 05, 13);
            DateTime rangeEnd = new DateTime(2024, 05, 17);

            List<Lesson> inRangeLessons = new List<Lesson>()
            {
                new Lesson(1, "Lesson1", new DateTime(2024, 05, 13, 8,0,0), 30, teacher, LicenceType.Car, car, student1),
                new Lesson(2, "Lesson2", new DateTime(2024, 05, 17, 18, 30, 0), 30, teacher, LicenceType.Car, car, student2),
                new Lesson(3, "Lesson3", new DateTime(2024, 05, 15, 12, 30, 0), 30, teacher, LicenceType.Car, car, student2),
                new Lesson(4, "Lesson4", new DateTime(2024, 05, 15, 10, 30, 0), 30, teacher, LicenceType.Car, car, student2),
            };
            List<Lesson> outOfRangeLessons = new List<Lesson>()
            {
                new Lesson(5, "Lesson5", new DateTime(2024, 05, 12, 17, 0, 0), 30, teacher, LicenceType.Car, car, student1),
                new Lesson(6, "Lesson6", new DateTime(2024, 05, 18, 8, 0, 0), 30, teacher, LicenceType.Car, car, student2),
            };

            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(inRangeLessons);
            _lessonRepository.Insert(outOfRangeLessons);

            // Act
            var query = new GetUserPlanning_Query(teacherId, rangeStart, rangeEnd);
            List<Lesson> planning = await _mediator.Send(query);

            // Assert
            Assert.NotNull(planning);
            Assert.Equal(inRangeLessons, planning);
            Assert.DoesNotContain(outOfRangeLessons, l => planning.Contains(l));
        }

        [Fact]
        public async void ScheduleShould_NotGetPlanning_ForInvalidUser()
        {
            // Arrange
            Guid invalidStudentId = new Guid("00000000-0000-0000-0000-000000000001");
            DateTime rangeStart = new DateTime(2024, 05, 13);
            DateTime rangeEnd = new DateTime(2024, 05, 17);

            _userRepository.Insert(DataSet.GetCarStudent(Guid.NewGuid()));

            // Act
            var query = new GetUserPlanning_Query(invalidStudentId, rangeStart, rangeEnd);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(query));
        }

        [Fact]
        public async void ScheduleShould_NotGetOtherStudentsPlanning()
        {
            _clock.Set(new DateTime(2024, 05, 15));

            // Arrange
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid teacherId = new Guid("00000000-0000-0000-0000-000000000003");

            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User teacher = DataSet.GetCarTeacher(teacherId);
            Vehicle car = DataSet.GetCar(1);

            DateTime rangeStart = new DateTime(2024, 05, 13);
            DateTime rangeEnd = new DateTime(2024, 05, 17);

            List<Lesson> student1Lessons = new List<Lesson>()
            {
                new Lesson(1, "Lesson1", new DateTime(2024, 05, 13, 8, 0, 0), 30, teacher, LicenceType.Car, car, student1),
                new Lesson(2, "Lesson2", new DateTime(2024, 05, 17, 18, 30, 0), 30, teacher, LicenceType.Car, car, student1),
            };
            List<Lesson> student2Lessons = new List<Lesson>()
            {
                new Lesson(3, "Lesson3", new DateTime(2024, 05, 13, 10, 0, 0), 30, teacher, LicenceType.Car, car, student2),
                new Lesson(4, "Lesson4", new DateTime(2024, 05, 17, 15, 0, 0), 30, teacher, LicenceType.Car, car, student2),
            };

            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(student1Lessons);
            _lessonRepository.Insert(student2Lessons);

            // Act
            var query = new GetUserPlanning_Query(studentId1, rangeStart, rangeEnd);
            List<Lesson> planning = await _mediator.Send(query);

            // Assert
            Assert.NotNull(planning);
            Assert.Equal(student1Lessons.Count, planning.Count);
            Assert.Equal(student1Lessons, planning);
            Assert.DoesNotContain(student2Lessons, l => planning.Contains(l));
        }

        [Fact]
        public async void ScheduleShould_NotGetOtherTeachersPlanning()
        {
            _clock.Set(new DateTime(2024, 05, 15));

            // Arrange
            Guid studentId1 = new Guid("00000000-0000-0000-0000-000000000001");
            Guid studentId2 = new Guid("00000000-0000-0000-0000-000000000002");
            Guid teacherId1 = new Guid("00000000-0000-0000-0000-000000000003");
            Guid teacherId2 = new Guid("00000000-0000-0000-0000-000000000004");

            User student1 = DataSet.GetCarStudent(studentId1);
            User student2 = DataSet.GetCarStudent(studentId2);
            User teacher1 = DataSet.GetCarTeacher(teacherId1);
            User teacher2 = DataSet.GetCarTeacher(teacherId2);
            Vehicle car = DataSet.GetCar(1);

            DateTime rangeStart = new DateTime(2024, 05, 13);
            DateTime rangeEnd = new DateTime(2024, 05, 17);

            List<Lesson> teacher1Lessons = new List<Lesson>()
            {
                new Lesson(1, "Lesson1", new DateTime(2024, 05, 13, 08, 0,0), 30, teacher1, LicenceType.Car, car, student1),
                new Lesson(2, "Lesson2", new DateTime(2024, 05, 14, 18, 30, 0), 30, teacher1, LicenceType.Car, car, student1),
                new Lesson(3, "Lesson3", new DateTime(2024, 05, 15, 08, 0, 0), 30, teacher1, LicenceType.Car, car, student2),
                new Lesson(4, "Lesson4", new DateTime(2024, 05, 17, 18, 30, 0), 30, teacher1, LicenceType.Car, car, student2),
            };
            List<Lesson> teacher2Lessons = new List<Lesson>()
            {
                new Lesson(5, "Lesson5", new DateTime(2024, 05, 14, 10, 0, 0), 30, teacher2, LicenceType.Car, car, student1),
                new Lesson(6, "Lesson6", new DateTime(2024, 05, 16, 15, 0, 0), 30, teacher2, LicenceType.Car, car, student1),
                new Lesson(7, "Lesson7", new DateTime(2024, 05, 16, 10, 0, 0), 30, teacher2, LicenceType.Car, car, student2),
                new Lesson(8, "Lesson8", new DateTime(2024, 05, 17, 15, 0, 0), 30, teacher2, LicenceType.Car, car, student2),
            };

            _userRepository.Insert(student1);
            _userRepository.Insert(student2);
            _userRepository.Insert(teacher1);
            _userRepository.Insert(teacher2);
            _vehicleRepository.Insert(car);
            _lessonRepository.Insert(teacher1Lessons);
            _lessonRepository.Insert(teacher2Lessons);            

            // Act
            var query = new GetUserPlanning_Query(teacherId1, rangeStart, rangeEnd);
            List<Lesson> planning = await _mediator.Send(query);

            // Assert
            Assert.NotNull(planning);
            Assert.Equal(teacher1Lessons.Count, planning.Count);
            Assert.Equal(teacher1Lessons, planning);
            Assert.DoesNotContain(teacher2Lessons, l => planning.Contains(l));
        }
    }
}
