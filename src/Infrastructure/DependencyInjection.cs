using Domain.Abstractions;

using Infrastructure.ExternalServices;
using Infrastructure.Persistence;

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
        }

        public static void SetupDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IDatabase, DatabaseContext>(options => options.UseSqlite(connectionString));
        }

        public static void SetupInMemoryDatabase(this IServiceCollection services, string inMemoryDbName)
        {
            services.AddDbContext<IDatabase, DatabaseContext>(options => options.UseInMemoryDatabase(databaseName: inMemoryDbName));
        }
    }
}
