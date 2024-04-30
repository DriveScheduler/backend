using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void ApplicationMediator(this IServiceCollection services)
        {
            services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        }

    }
}
