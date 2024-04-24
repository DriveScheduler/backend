using Domain;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public sealed record GetWeatherForecast_Query() : IRequest<List<WeatherForecast>>;

    internal sealed class GetWeatherForecast_QueryHandler : IRequestHandler<GetWeatherForecast_Query, List<WeatherForecast>>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<List<WeatherForecast>> Handle(GetWeatherForecast_Query request, CancellationToken cancellationToken)
        {
            List<WeatherForecast> list = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();

            return Task.FromResult(list);
        }
    }
}
