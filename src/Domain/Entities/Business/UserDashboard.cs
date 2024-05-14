using Domain.Entities.Database;

namespace Domain.Entities.Business
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
