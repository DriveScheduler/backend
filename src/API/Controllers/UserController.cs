using API.Inputs.Users;

using Application.UseCases.Users.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            var command = new CreateUser_Command(model.Name, model.FirstName, model.Email, model.LicenceType);
            try
            {
                Guid userId = await _mediator.Send(command);
                return Ok(userId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
