using API.Outputs.Lessons;
using API.Outputs.Vehicles;

using Application.Models;

namespace API.Outputs.Users
{
    public sealed class UserDashboardOutput
    {
        public LessonLight? NextLesson { get; }
        public LessonLight? LastLesson { get; }
        public UserLight? FavoriteUser { get; init; }
        public int FavoriteUserTimeSpent { get; init; }
        public VehicleLight? FavoriteVehicle { get; init; }
        public int FavoriteVehicleTimeSpent { get; init; }
        public int TimeSpentThisWeek { get; init; }

        public UserDashboardOutput(UserDashboard dashboard)
        {
            NextLesson = dashboard.NextLesson != null ? new LessonLight(dashboard.NextLesson) : null;
            LastLesson = dashboard.LastLesson != null ? new LessonLight(dashboard.LastLesson) : null;

            FavoriteUser = dashboard.FavoriteUser != null ? new UserLight(dashboard.FavoriteUser) : null;
            FavoriteUserTimeSpent = dashboard.FavoriteUserTimeSpent;

            FavoriteVehicle = dashboard.FavoriteVehicle != null ? new VehicleLight(dashboard.FavoriteVehicle) : null;
            FavoriteVehicleTimeSpent = dashboard.FavoriteVehicleTimeSpent;

            TimeSpentThisWeek = dashboard.TimeSpentThisWeek;
        }
    }
}
