using Application.Abstractions;
using Application.Models;

using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserDashboard_Query(Guid UserId) : IRequest<UserDashboard>;

    internal sealed class GetUserDashboard_QueryHandler(
        IUserRepository userRepository,
        ISystemClock clock
        ) : IRequestHandler<GetUserDashboard_Query, UserDashboard>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _clock = clock;

        public Task<UserDashboard> Handle(GetUserDashboard_Query request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserById(request.UserId);

            IReadOnlyList<Lesson> allLessons = [];
            List<Lesson> achievedLessons = [];
            User? favouriteUser = null;
            int userTotalTime = 0;

            if (user is Student student)
            {
                allLessons = student.Lessons;
                achievedLessons = allLessons.Where(l => l.End <= _clock.Now).ToList();
                favouriteUser = FavouriteTeacher(achievedLessons, out userTotalTime);
            }
            else if(user is Teacher teacher)
            {
                allLessons = teacher.Lessons;
                achievedLessons = allLessons.Where(l => l.End <= _clock.Now).ToList();
                favouriteUser = FavoriteStudent(achievedLessons, out userTotalTime);
            }           

            Vehicle? favouriteVehicle = FavouriteVehicle(achievedLessons, out int vehicleTotalTime);

            DateTime firstDayOfThisWeek = _clock.Now.AddDays(-1 * (_clock.Now.DayOfWeek - DayOfWeek.Monday)).Date;

            UserDashboard dashboard = new UserDashboard()
            {
                NextLesson = allLessons.Where(lesson => lesson.Start > _clock.Now).OrderBy(lesson => lesson.Start).FirstOrDefault(),
                LastLesson = allLessons.Where(lesson => lesson.End < _clock.Now).OrderByDescending(lesson => lesson.Start).FirstOrDefault(),
                FavoriteUser = favouriteUser,
                FavoriteUserTimeSpent = userTotalTime,
                FavoriteVehicle = favouriteVehicle,
                FavoriteVehicleTimeSpent = vehicleTotalTime,
                TimeSpentThisWeek = allLessons.Where(lesson => lesson.Start.Date >= firstDayOfThisWeek && lesson.End < _clock.Now).Sum(lesson => lesson.Duration.Value)
            };

            return Task.FromResult(dashboard);
        }

        private User? FavoriteStudent(List<Lesson> teacherLesson, out int totalTime)
        {
            Dictionary<User, List<Lesson>> studentLessons = teacherLesson.GroupBy(l => l.Student).ToDictionary(row => (User)row.Key, row => row.ToList());
            return FavoriteUser(studentLessons, out totalTime);
        }

        private User? FavouriteTeacher(List<Lesson> studentLessons, out int totalTime)
        {
            Dictionary<User, List<Lesson>> teacherLessons = studentLessons.GroupBy(l => l.Teacher).ToDictionary(row => (User)row.Key, row => row.ToList());
            return FavoriteUser(teacherLessons, out totalTime);
            //totalTime = 0;
            //User? favoriteTeacher = null;

            //Dictionary<Teacher, List<Lesson>> teacherLessons = studentLessons.GroupBy(l => l.Teacher).ToDictionary(row => row.Key, row => row.ToList());
            //if (teacherLessons.Count == 1)
            //{
            //    totalTime = teacherLessons.First().Value.Sum(l => l.Duration.Value);
            //    return teacherLessons.First().Key;
            //}

            //int maxDuration = 0;
            //foreach (KeyValuePair<Teacher, List<Lesson>> row in teacherLessons)
            //{
            //    int total = row.Value.Sum(l => l.Duration.Value);
            //    if (total > maxDuration)
            //    {
            //        maxDuration = total;
            //        favoriteTeacher = row.Key;
            //    }
            //}

            //totalTime = maxDuration;
            //return favoriteTeacher;
        }

        private User? FavoriteUser(Dictionary<User, List<Lesson>> userLessons, out int totalTime)
        {
            User? favoriteUser = null;        
            totalTime = 0;
            if (userLessons.Count == 1)
            {
                totalTime = userLessons.First().Value.Sum(l => l.Duration.Value);
                return userLessons.First().Key;
            }

            int maxDuration = 0;
            foreach (KeyValuePair<User, List<Lesson>> row in userLessons)
            {
                int total = row.Value.Sum(l => l.Duration.Value);
                if (total > maxDuration)
                {
                    maxDuration = total;
                    favoriteUser = row.Key;
                }
            }

            totalTime = maxDuration;
            return favoriteUser;
        }

        private Vehicle? FavouriteVehicle(List<Lesson> studentLessons, out int totalTime)
        {
            totalTime = 0;
            Vehicle? favoriteVehicle = null;

            Dictionary<Vehicle, List<Lesson>> vehicleLessons = studentLessons.GroupBy(l => l.Vehicle).ToDictionary(row => row.Key, row => row.ToList());
            if (vehicleLessons.Count == 1)
            {
                totalTime = vehicleLessons.First().Value.Sum(l => l.Duration.Value);
                return vehicleLessons.First().Key;
            }

            int maxDuration = 0;
            foreach (KeyValuePair<Vehicle, List<Lesson>> row in vehicleLessons)
            {
                int total = row.Value.Sum(l => l.Duration.Value);
                if (total > maxDuration)
                {
                    maxDuration = total;
                    favoriteVehicle = row.Key;
                }
            }

            totalTime = maxDuration;
            return favoriteVehicle;
        }
    }

}
