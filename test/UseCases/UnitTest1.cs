using Application;

using Domain;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace UseCases
{

    public class SetupMediator
    {
        public IServiceProvider ServiceProvider { get; }

        public SetupMediator()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.ApplicationDependencyInjection();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }


    public class UnitTest1 : IClassFixture<SetupMediator>
    {
        private IServiceProvider _serviceProvider;

        public UnitTest1(SetupMediator fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }


        [Fact]
        public async void Test1()
        {
            IMediator mediator = _serviceProvider.GetRequiredService<IMediator>();

            List<WeatherForecast> forecasts = await mediator.Send(new GetWeatherForecast_Query());

            Assert.NotNull(forecasts);
            Assert.Equal(5, forecasts.Count);

        }
    }
}