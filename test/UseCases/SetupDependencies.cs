using Application;
using Application.Abstractions;

using Infrastructure;
using Infrastructure.Persistence;

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
            _serviceCollection.AddRepositories();

            _serviceCollection.SetupInMemoryDatabase(Guid.NewGuid().ToString());
            
            AddFakeSystemClock();  
            AddFakeEmailSender();
            //AddFakeDataAccessor();

            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }        

        private void AddFakeSystemClock()
        {
            _serviceCollection.AddSingleton<ISystemClock>(new FakeSystemClock(new DateTime(2024, 04, 25, 8, 30, 00)));
        }

        private void AddFakeEmailSender()
        {
            _serviceCollection.AddScoped<IEmailSender, FakeEmailSender>();
        }

        private void AddFakeDataAccessor()
        {
            _serviceCollection.AddTransient<IDataAccessor, FakeDataAccessor>();
        }
    }
}
