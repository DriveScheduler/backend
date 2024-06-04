using Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace Integration
{
    public class SetupDependencies
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected readonly IServiceCollection _serviceCollection;

        public SetupDependencies()
        {
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddRepositories();
            _serviceCollection.SetupInMemoryDatabase(Guid.NewGuid().ToString());

            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }  
    }
}
