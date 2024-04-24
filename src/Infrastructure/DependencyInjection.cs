using Application.Abstractions;

using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void InfrastructureDependencyInjection(this IServiceCollection services)
        {
        }

        public static void SetupDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IDatabase, DatabaseContexte>(options => options.UseSqlite(connectionString));
        }

        public static void  SetupDatabaseInMemory(this IServiceCollection services)
        {
            services.AddDbContext<IDatabase, DatabaseContexte>(options => options.UseInMemoryDatabase(databaseName: "TestDatabase"));
        }
    }
}
