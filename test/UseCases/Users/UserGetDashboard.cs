using Application.Abstractions;
using Application.UseCases.Users.Queries;

using Domain.Models;
using Application.Models;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.Fakes;
using UseCases.TestData;
using Infrastructure.Persistence;
using Domain.Models.Users;
using Domain.Models.Vehicles;

namespace UseCases.Users
{
    public class UserGetDashboard : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IDataAccessor _database;
        private readonly IMediator _mediator;
        private readonly FakeSystemClock _clock;

        public UserGetDashboard(SetupDependencies fixture)
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
        public async void UserShould_GetHisLessonHistory()
        {
            PlanningDataSet dataset = new PlanningDataSet();
            _clock.Set(dataset.Date);

            // Arrange
            User user = dataset.Student;

            _userRepository.Insert(user);
            _userRepository.Insert(dataset.GetAllTeachers());
            _vehicleRepository.Insert(dataset.GetAllVehicles());
            _lessonRepository.Insert(dataset.GetAllLessons());

            // Act
            var command = new GetUserLessonHistory_Query(user.Id);
            UserLessonHistory history = await _mediator.Send(command);

            // Assert
            List<Lesson> expected = dataset.AchievedLessons();
            int expectedTotalTime = dataset.AchievedLessonsTotalTime();
            Assert.NotNull(history);
            Assert.Equal(expected.Count, history.Lessons.Count);
            Assert.Equal(expectedTotalTime, history.LessonTotalTime);
        }

        [Fact]
        public async void SystemShould_NotGetLessonHistory_ForInvalidUser()
        {
            PlanningDataSet dataset = new PlanningDataSet();
            _clock.Set(dataset.Date);

            // Arrange
            User user = DataSet.GetCarStudent(Guid.NewGuid());

            _userRepository.Insert(dataset.Student);
            _userRepository.Insert(dataset.GetAllTeachers());
            _vehicleRepository.Insert(dataset.GetAllVehicles());
            _lessonRepository.Insert(dataset.GetAllLessons());

            // Act
            var command = new GetUserLessonHistory_Query(user.Id);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
        }

        [Fact]
        public async void UserShould_GetHisLessonPlanning()
        {
            PlanningDataSet dataset = new PlanningDataSet();
            _clock.Set(dataset.Date);

            // Arrange
            User user = dataset.Student;

            _userRepository.Insert(user);
            _userRepository.Insert(dataset.GetAllTeachers());
            _vehicleRepository.Insert(dataset.GetAllVehicles());
            _lessonRepository.Insert(dataset.GetAllLessons());

            // Act
            var command = new GetUserLessonPlanning_Query(user.Id);
            UserLessonPlanning planning = await _mediator.Send(command);

            // Assert
            List<Lesson> expectedTodayLessons = dataset.TodayLessons();
            List<Lesson> expectedTomorrowLessons = dataset.TomorrowLessons();
            List<Lesson> expectedWeekLessons = dataset.ThisWeekLessons();
            List<Lesson> expectedMonthLessons = dataset.ThisMonthLessons();
            List<Lesson> expectedNextMonthsLessons = dataset.NextMonthsLessons();
            int expectedTotalLessons = dataset.NextLessonsTotal();
            int expectedTotalTime = dataset.NextLessonsTotalTime();
            Assert.NotNull(planning);
            Assert.Equal(expectedTotalLessons, planning.TotalLessons);
            Assert.Equal(expectedTotalTime, planning.TotalTime);

            Assert.Equal(expectedTodayLessons, planning.Today);
            Assert.Equal(expectedTomorrowLessons, planning.Tomorrow);
            Assert.Equal(expectedWeekLessons, planning.ThisWeek);
            Assert.Equal(expectedMonthLessons, planning.ThisMonth);
            Assert.Equal(expectedNextMonthsLessons, planning.NextMonths);
        }

        [Fact]
        public async void SystemShould_NotGetLessonPlanning_ForInvalidUser()
        {
            PlanningDataSet dataset = new PlanningDataSet();
            _clock.Set(dataset.Date);

            // Arrange
            User user = DataSet.GetCarStudent(Guid.NewGuid());

            _userRepository.Insert(dataset.Student);
            _userRepository.Insert(dataset.GetAllTeachers());
            _vehicleRepository.Insert(dataset.GetAllVehicles());
            _lessonRepository.Insert(dataset.GetAllLessons());

            // Act
            var command = new GetUserLessonPlanning_Query(user.Id);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
        }

        [Fact]
        public async void UserShould_GetHisDashboard()
        {
            PlanningDataSet dataset = new PlanningDataSet();
            _clock.Set(dataset.Date);

            // Arrange
            User user = dataset.Student;

            _userRepository.Insert(user);
            _userRepository.Insert(dataset.GetAllTeachers());
            _vehicleRepository.Insert(dataset.GetAllVehicles());
            _lessonRepository.Insert(dataset.GetAllLessons());

            // Act
            var command = new GetUserDashboard_Query(user.Id);
            UserDashboard dashboard = await _mediator.Send(command);

            // Assert
            Lesson expectedNextLesson = dataset.NextLesson();
            Lesson expectedLastLessons = dataset.LastLesson();
            User expectedFavoriteTeacher = dataset.FavoriteTeacher();
            int expectedFavoriteTeacherTime = dataset.FavoriteTeacherTotalTime();
            Vehicle expectedFavoriteVehicle = dataset.FavoriteVehicle();
            int expectedFavoriteVehicleTime = dataset.FavoriteVehicleTotalTime();
            int expectedTotalLessonTimeThisWeek = dataset.LessonTotalTimeThisWeek();

            Assert.NotNull(dashboard);
            Assert.Equal(expectedNextLesson, dashboard.NextLesson);
            Assert.Equal(expectedLastLessons, dashboard.LastLesson);

            Assert.Equal(expectedFavoriteTeacher, dashboard.FavoriteTeacher);
            Assert.Equal(expectedFavoriteTeacherTime, dashboard.FavoriteTeacherTimeSpent);

            Assert.Equal(expectedFavoriteVehicle, dashboard.FavoriteVehicle);
            Assert.Equal(expectedFavoriteVehicleTime, dashboard.FavoriteVehicleTimeSpent);

            Assert.Equal(expectedTotalLessonTimeThisWeek, dashboard.TimeSpentThisWeek);
        }

        [Fact]
        public async void SystemShould_NotGetDashboard_ForInvalidUser()
        {
            PlanningDataSet dataset = new PlanningDataSet();
            _clock.Set(dataset.Date);

            // Arrange
            User user = DataSet.GetCarStudent(Guid.NewGuid());

            _userRepository.Insert(dataset.Student);
            _userRepository.Insert(dataset.GetAllTeachers());
            _vehicleRepository.Insert(dataset.GetAllVehicles());
            _lessonRepository.Insert(dataset.GetAllLessons());

            // Act
            var command = new GetUserDashboard_Query(user.Id);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
        }


    }
}
