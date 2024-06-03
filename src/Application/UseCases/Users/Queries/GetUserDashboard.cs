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
        ILessonRepository lessonRepository,  
        IUserRepository userRepository,
        ISystemClock clock
        ) : IRequestHandler<GetUserDashboard_Query, UserDashboard>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;  
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _clock = clock;

        public Task<UserDashboard> Handle(GetUserDashboard_Query request, CancellationToken cancellationToken)
        {
            Student student = _userRepository.GetStudentById(request.UserId);
            IReadOnlyList<Lesson> allStudentLessons = student.Lessons;

            List<Lesson> achievedLessons = allStudentLessons.Where(l => l.End < _clock.Now).ToList();
            User? favouriteTeacher = FavouriteTeacher(achievedLessons, out int teacherTotalTime);
            Vehicle? favouriteVehicle = FavouriteVehicle(achievedLessons, out int vehicleTotalTime);
            
            DateTime firstDayOfThisWeek = _clock.Now.AddDays(-1 * (_clock.Now.DayOfWeek - DayOfWeek.Monday)).Date;

            UserDashboard dashboard = new UserDashboard()
            {
                NextLesson = allStudentLessons.Where(lesson => lesson.Start > _clock.Now).OrderBy(lesson => lesson.Start).FirstOrDefault(),
                LastLesson = allStudentLessons.Where(lesson => lesson.End < _clock.Now).OrderByDescending(lesson => lesson.Start).FirstOrDefault(),
                FavoriteTeacher = favouriteTeacher,
                FavoriteTeacherTimeSpent = teacherTotalTime,
                FavoriteVehicle = favouriteVehicle,
                FavoriteVehicleTimeSpent = vehicleTotalTime,
                TimeSpentThisWeek = allStudentLessons.Where(lesson => lesson.Start.Date >= firstDayOfThisWeek && lesson.End < _clock.Now).Sum(lesson => lesson.Duration.Value)
            };

            return Task.FromResult(dashboard);
        }

        private User? FavouriteTeacher(List<Lesson> studentLessons, out int totalTime)
        {
            totalTime = 0;
            User? favoriteTeacher = null;

            Dictionary<Teacher, List<Lesson>> teacherLessons = studentLessons.GroupBy(l => l.Teacher).ToDictionary(row => row.Key, row => row.ToList());
            if (teacherLessons.Count == 1)
            {
                totalTime = teacherLessons.First().Value.Sum(l => l.Duration.Value);
                return teacherLessons.First().Key;
            }

            int maxDuration = 0;
            foreach (KeyValuePair<Teacher, List<Lesson>> row in teacherLessons)
            {
                int total = row.Value.Sum(l => l.Duration.Value);
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
