using Domain.Enums;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenceController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public IActionResult Licences()
        {
            return Ok(Enum.GetValues(typeof(LicenceType)).Cast<LicenceType>().Select(x => new { Id = (int)x, Name = x.ToText() }));
        }
    }
}