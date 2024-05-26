using Application.Abstractions;

using Domain.Entities;
using Domain.Entities.Business;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserDashboard_Query(Guid UserId) : IRequest<UserDashboard>;

    internal sealed class GetUserDashboard_QueryHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        IVehicleRepository vehicleRepository,
        ISystemClock clock) : IRequestHandler<GetUserDashboard_Query, UserDashboard>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ISystemClock _clock = clock;

        public async Task<UserDashboard> Handle(GetUserDashboard_Query request, CancellationToken cancellationToken)
        {
            User? student = _database.Users.Find(request.UserId);
            if (student is null)
                throw new UserNotFoundException();

            List<Lesson> allStudentLessons = await _database.Lessons
                .Include(l => l.Student)
                .Where(l => l.Student == student)
                .ToListAsync();

            List<Lesson> achievedLessons = allStudentLessons.Where(l => l.End < _clock.Now).ToList();
            User? favouriteTeacher = FavouriteTeacher(achievedLessons, out int teacherTotalTime);
            Vehicle? favouriteVehicle = FavouriteVehicle(achievedLessons, out int vehicleTotalTime);

            DateTime firstDayOfThisWeek = DateUtil.GetFirstDayOfWeek(_clock.Now);

            UserDashboard dashboard = new UserDashboard()
            {
                NextLesson = allStudentLessons.Where(lesson => lesson.Start > _clock.Now).OrderBy(lesson => lesson.Start).FirstOrDefault(),
                LastLesson = allStudentLessons.Where(lesson => lesson.End < _clock.Now).OrderByDescending(lesson => lesson.Start).FirstOrDefault(),
                FavoriteTeacher = favouriteTeacher,
                FavoriteTeacherTimeSpent = teacherTotalTime,
                FavoriteVehicle = favouriteVehicle,
                FavoriteVehicleTimeSpent = vehicleTotalTime,
                TimeSpentThisWeek = allStudentLessons.Where(lesson => lesson.Start.Date >= firstDayOfThisWeek && lesson.End < _clock.Now).Sum(lesson => lesson.Duration)
            };

            return dashboard;
        }     

        private User? FavouriteTeacher(List<Lesson> studentLessons, out int totalTime)
        {
            totalTime = 0;
            User? favoriteTeacher = null;

            Dictionary<User, List<Lesson>> teacherLessons = studentLessons.GroupBy(l => l.Teacher).ToDictionary(row => row.Key, row => row.ToList());
            if (teacherLessons.Count == 1)
            {
                totalTime = teacherLessons.First().Value.Sum(l => l.Duration);
                return teacherLessons.First().Key;
            }

            int maxDuration = 0;
            foreach (KeyValuePair<User, List<Lesson>> row in teacherLessons)
            {
                int total = row.Value.Sum(l => l.Duration);
                if (total > maxDuration)
                {
                    maxDuration = total;
                    favoriteTeacher = row.Key;
                }
            }

            totalTime = maxDuration;
            return favoriteTeacher;
        }
        private Vehicle? FavouriteVehicle(List<Lesson> studentLessons, out int totalTime)
        {
            totalTime = 0;
            Vehicle? favoriteVehicle = null;

            Dictionary<Vehicle, List<Lesson>> vehicleLessons = studentLessons.GroupBy(l => l.Vehicle).ToDictionary(row => row.Key, row => row.ToList());
            if (vehicleLessons.Count == 1)
            {
                totalTime = vehicleLessons.First().Value.Sum(l => l.Duration);
                return vehicleLessons.First().Key;
            }

            int maxDuration = 0;
            foreach (KeyValuePair<Vehicle, List<Lesson>> row in vehicleLessons)
            {
                int total = row.Value.Sum(l => l.Duration);
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
