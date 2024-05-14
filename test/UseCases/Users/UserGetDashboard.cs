using Application.UseCases.Users.Queries;

using Domain.Abstractions;
using Domain.Entities.Business;
using Domain.Entities.Database;
using Domain.Exceptions.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using UseCases.Fakes;
using UseCases.TestData;

namespace UseCases.Users
{
    public class UserGetDashboard : IClassFixture<SetupDependencies>, IDisposable
    {
        private IMediator _mediator;
        private IDatabase _database;
        private FakeSystemClock _clock;

        public UserGetDashboard(SetupDependencies fixture)
        {
            _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
            _database = fixture.ServiceProvider.GetRequiredService<IDatabase>();
            _clock = (FakeSystemClock)fixture.ServiceProvider.GetRequiredService<ISystemClock>();
        }

        public void Dispose()
        {
            _database.Clear();
        }      

        [Fact]
        public async void UserShould_GetHisLessonHistory()
        {
            _clock.Set(PlanningDataSet.Date);

            // Arrange
            User user = PlanningDataSet.Student;
            
            _database.Users.Add(user);
            _database.Users.AddRange(PlanningDataSet.GetAllTeachers());
            _database.Vehicles.AddRange(PlanningDataSet.GetAllVehicles());
            _database.Lessons.AddRange(PlanningDataSet.GetAllLessons());
            await _database.SaveChangesAsync();

            // Act
            var command = new GetUserLessonHistory_Query(user.Id);
            UserLessonHistory history = await _mediator.Send(command);

            // Assert
            List<Lesson> expected = PlanningDataSet.AchievedLessons();
            int expectedTotalTime = PlanningDataSet.AchievedLessonsTotalTime();
            Assert.NotNull(history);
            Assert.Equal(expected.Count, history.Lessons.Count);
            Assert.Equal(expectedTotalTime, history.LessonTotalTime);          
        }

        [Fact]
        public async void SystemShould_NotGetLessonHistory_ForInvalidUser()
        {
            _clock.Set(PlanningDataSet.Date);

            // Arrange
            User user = DataSet.GetCarStudent(Guid.NewGuid());

            _database.Users.Add(PlanningDataSet.Student);
            _database.Users.AddRange(PlanningDataSet.GetAllTeachers());
            _database.Vehicles.AddRange(PlanningDataSet.GetAllVehicles());
            _database.Lessons.AddRange(PlanningDataSet.GetAllLessons());
            await _database.SaveChangesAsync();

            // Act
            var command = new GetUserLessonHistory_Query(user.Id);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>( () => _mediator.Send(command));           
        }

        [Fact]
        public async void UserShould_GetHisLessonPlanning()
        {
            _clock.Set(PlanningDataSet.Date);

            // Arrange
            User user = PlanningDataSet.Student;

            _database.Users.Add(user);
            _database.Users.AddRange(PlanningDataSet.GetAllTeachers());
            _database.Vehicles.AddRange(PlanningDataSet.GetAllVehicles());
            _database.Lessons.AddRange(PlanningDataSet.GetAllLessons());
            await _database.SaveChangesAsync();

            // Act
            var command = new GetUserLessonPlanning_Query(user.Id);
            UserLessonPlanning planning = await _mediator.Send(command);

            // Assert
            List<Lesson> expectedTodayLessons = PlanningDataSet.TodayLessons();
            List<Lesson> expectedTomorrowLessons = PlanningDataSet.TomorrowLessons();
            List<Lesson> expectedWeekLessons = PlanningDataSet.ThisWeekLessons();
            List<Lesson> expectedMonthLessons = PlanningDataSet.ThisMonthLessons();
            List<Lesson> expectedNextMonthsLessons = PlanningDataSet.NextMonthsLessons();
            int expectedTotalLessons = PlanningDataSet.NextLessonsTotal();
            int expectedTotalTime = PlanningDataSet.NextLessonsTotalTime();
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
            _clock.Set(PlanningDataSet.Date);

            // Arrange
            User user = DataSet.GetCarStudent(Guid.NewGuid());

            _database.Users.Add(PlanningDataSet.Student);
            _database.Users.AddRange(PlanningDataSet.GetAllTeachers());
            _database.Vehicles.AddRange(PlanningDataSet.GetAllVehicles());
            _database.Lessons.AddRange(PlanningDataSet.GetAllLessons());
            await _database.SaveChangesAsync();

            // Act
            var command = new GetUserLessonPlanning_Query(user.Id);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
        }

        [Fact]
        public async void UserShould_GetHisDashboard()
        {
            _clock.Set(PlanningDataSet.Date);

            // Arrange
            User user = PlanningDataSet.Student;

            _database.Users.Add(user);
            _database.Users.AddRange(PlanningDataSet.GetAllTeachers());
            _database.Vehicles.AddRange(PlanningDataSet.GetAllVehicles());
            _database.Lessons.AddRange(PlanningDataSet.GetAllLessons());
            await _database.SaveChangesAsync();

            // Act
            var command = new GetUserDashboard_Query(user.Id);
            UserDashboard dashboard = await _mediator.Send(command);

            // Assert
            Lesson expectedNextLesson = PlanningDataSet.NextLesson();
            Lesson expectedLastLessons = PlanningDataSet.LastLesson();
            User expectedFavoriteTeacher = PlanningDataSet.FavoriteTeacher();
            int expectedFavoriteTeacherTime = PlanningDataSet.FavoriteTeacherTotalTime();
            Vehicle expectedFavoriteVehicle = PlanningDataSet.FavoriteVehicle();
            int expectedFavoriteVehicleTime = PlanningDataSet.FavoriteVehicleTotalTime();
            int expectedTotalLessonTimeThisWeek = PlanningDataSet.LessonTotalTimeThisWeek();
            
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
            _clock.Set(PlanningDataSet.Date);

            // Arrange
            User user = DataSet.GetCarStudent(Guid.NewGuid());

            _database.Users.Add(PlanningDataSet.Student);
            _database.Users.AddRange(PlanningDataSet.GetAllTeachers());
            _database.Vehicles.AddRange(PlanningDataSet.GetAllVehicles());
            _database.Lessons.AddRange(PlanningDataSet.GetAllLessons());
            await _database.SaveChangesAsync();

            // Act
            var command = new GetUserDashboard_Query(user.Id);

            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _mediator.Send(command));
        }


    }
}
