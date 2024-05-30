using API.Outputs;

using Domain.Enums;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Licences()
        {            
            return Ok(Enum.GetValues(typeof(LicenceType)).Cast<LicenceType>().Select(x => new LicenceTypeOutput(x)));
        }
    }
}