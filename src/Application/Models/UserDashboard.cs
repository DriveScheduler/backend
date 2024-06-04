using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;

namespace Application.Models
{
    public sealed class UserDashboard
    {
        public Lesson? NextLesson { get; init; }
        public Lesson? LastLesson { get; init; }
        public User? FavoriteTeacher { get; init; }
        public int FavoriteTeacherTimeSpent { get; init; }
        public Vehicle? FavoriteVehicle { get; init; }
        public int FavoriteVehicleTimeSpent { get; init; }
        public int TimeSpentThisWeek { get; init; }
    }
}
