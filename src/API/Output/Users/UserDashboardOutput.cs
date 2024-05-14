using API.Output.Lessons;
using API.Output.Vehicles;

using Domain.Entities.Business;

namespace API.Output.Users
{
    public sealed class UserDashboardOutput
    {
        public LessonLight? NextLesson { get; }
        public LessonLight? LastLesson { get; }
        public UserLight? FavoriteTeacher { get; init; }
        public int FavoriteTeacherTimeSpent { get; init; }
        public VehicleLight? FavoriteVehicle { get; init; }
        public int FavoriteVehicleTimeSpent { get; init; }
        public int TimeSpentThisWeek { get; init; }

        public UserDashboardOutput(UserDashboard dashboard)
        {
            NextLesson = dashboard.NextLesson != null ? new LessonLight(dashboard.NextLesson) : null;
            LastLesson = dashboard.LastLesson != null ? new LessonLight(dashboard.LastLesson) : null;

            FavoriteTeacher = dashboard.FavoriteTeacher != null ? new UserLight(dashboard.FavoriteTeacher) : null;
            FavoriteTeacherTimeSpent = dashboard.FavoriteTeacherTimeSpent;

            FavoriteVehicle = dashboard.FavoriteVehicle != null ? new VehicleLight(dashboard.FavoriteVehicle) : null;
            FavoriteVehicleTimeSpent = dashboard.FavoriteVehicleTimeSpent;

            TimeSpentThisWeek = dashboard.TimeSpentThisWeek;
        }
    }
}
