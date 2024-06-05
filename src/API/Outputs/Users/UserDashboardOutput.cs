using API.Outputs.Lessons;
using API.Outputs.Vehicles;

using Application.Models;

using Domain.Models.Users;

namespace API.Outputs.Users
{
    public sealed class UserDashboardOutput
    {
        public LessonDetail? NextLesson { get; }
        public LessonDetail? LastLesson { get; }
        public UserLight? FavoriteUser { get; init; }
        public int FavoriteUserTimeSpent { get; init; }
        public VehicleLight? FavoriteVehicle { get; init; }
        public int FavoriteVehicleTimeSpent { get; init; }
        public int TimeSpentThisWeek { get; init; }

        public UserDashboardOutput(UserDashboard dashboard, User connectedUser)
        {
            NextLesson = dashboard.NextLesson != null ? new LessonDetail(dashboard.NextLesson, connectedUser) : null;
            LastLesson = dashboard.LastLesson != null ? new LessonDetail(dashboard.LastLesson, connectedUser) : null;

            FavoriteUser = dashboard.FavoriteUser != null ? new UserLight(dashboard.FavoriteUser) : null;
            FavoriteUserTimeSpent = dashboard.FavoriteUserTimeSpent;

            FavoriteVehicle = dashboard.FavoriteVehicle != null ? new VehicleLight(dashboard.FavoriteVehicle) : null;
            FavoriteVehicleTimeSpent = dashboard.FavoriteVehicleTimeSpent;

            TimeSpentThisWeek = dashboard.TimeSpentThisWeek;
        }
    }
}
