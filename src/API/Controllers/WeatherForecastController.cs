using Application;

using Domain;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IMediator mediator) : ControllerBase
    {
        
        private readonly IMediator _mediator = mediator;       


        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            List<WeatherForecast> forecasts = await _mediator.Send(new GetWeatherForecast_Query());
            return Ok(forecasts);
        }
    }
}
