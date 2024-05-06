using API.Inputs.Users;

using Application.UseCases.Users.Commands;
using Application.UseCases.Users.Queries;

using Domain.Entities;

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
        public async Task<IActionResult> Create(CreateUserModel input)
        {
            var command = new CreateUser_Command(input.Name, input.FirstName, input.Email, input.Password, input.LicenceType, input.Type);
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

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserModel input)
        {
            var command = new UpdateUser_Command(input.Id, input.Name, input.FirstName, input.Email, input.LicenceType);
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetUserById_Query(id);
            try
            {
                User user = await _mediator.Send(query);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
