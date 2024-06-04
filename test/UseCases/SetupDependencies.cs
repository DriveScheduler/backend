using Application;
using Application.Abstractions;

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
        }     
        
        public void BuildDefault()
        {
            AddDefaultDependencies();
            Build();
        }

        public void Build()
        {
            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        public SetupDependencies AddDefaultDependencies()
        {
            AddFakeSystemClock();
            AddRepositories();
            AddInMemoryDatabase();
            return this;
        }

        #region FAKES
        public SetupDependencies AddFakeSystemClock()
        {
            _serviceCollection.AddSingleton<ISystemClock>(new FakeSystemClock(new DateTime(2024, 04, 25, 8, 30, 00)));
            return this;
        }

        public SetupDependencies AddFakeEmailSender()
        {
            _serviceCollection.AddScoped<IEmailSender, FakeEmailSender>();
            return this;
        }       
        #endregion

        #region DATABASE

        public SetupDependencies AddInMemoryDatabase()
        {
            _serviceCollection.SetupInMemoryDatabase(Guid.NewGuid().ToString());
            return this;
        }

        public SetupDependencies AddRepositories()
        {
            _serviceCollection.AddRepositories();
            return this;
        }
        #endregion
    }
}
