using Application.Abstractions;

using Domain.Repositories;

using Infrastructure.ExternalServices;
using Infrastructure.Persistence;
using Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void InfrastructureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ISystemClock, SystemClock>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddScoped<IDrivingSchoolRepository, DrivingSchoolRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
        }

        public static void SetupDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connectionString));
        }

        public static void SetupInMemoryDatabase(this IServiceCollection services, string inMemoryDbName)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase(databaseName: inMemoryDbName));
        }
    }
}
