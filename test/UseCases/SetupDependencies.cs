using Application;

using Domain.Abstractions;

using Infrastructure;

using Microsoft.Extensions.DependencyInjection;

using UseCases.Fakes;

namespace UseCases
{
    public class SetupDependencies
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected readonly IServiceCollection _serviceCollection;

        public SetupDependencies()
        {            
            _serviceCollection = new ServiceCollection();

            _serviceCollection.ApplicationMediator();

            _serviceCollection.SetupInMemoryDatabase(Guid.NewGuid().ToString());
            
            AddFakeSystemClock();            

            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }        

        private void AddFakeSystemClock()
        {
            _serviceCollection.AddSingleton<ISystemClock>(new FakeSystemClock(new DateTime(2024, 04, 25, 8, 30, 00)));
        }
    }
}
