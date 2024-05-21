using API.Authorization;
using API.Inputs.Users;
using API.Outputs.Users;
using API.Outputs.Users;

using Application.UseCases.Users.Commands;
using Application.UseCases.Users.Queries;

using Domain.Entities.Business;
using Domain.Entities.Database;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator, JwtTokenProvider tokenProvider) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;

        [HttpPost("Create")]
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

        [HttpPut("Update")]
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
                return Ok(new UserLight(user));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Teachers")]
        public async Task<IActionResult> Teachers()
        {
            var query = new GetTeachers_Query();
            try
            {
                List<User> teachers = await _mediator.Send(query);
                return Ok(teachers.Select(teacher => new UserLight(teacher)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/Dashboard")]
        public async Task<IActionResult> GetUserDashboard(Guid id)
        {
            var query = new GetUserDashboard_Query(id);
            try
            {
                UserDashboard dashboard = await _mediator.Send(query);
                return Ok(new UserDashboardOutput(dashboard));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }            
        }

        [HttpGet("{id}/Planning")]
        public async Task<IActionResult> GetUserPlanning(Guid id)
        {
            var query = new GetUserLessonPlanning_Query(id);
            try
            {
                UserLessonPlanning planning = await _mediator.Send(query);
                return Ok(new UserLessonPlanningOutput(planning));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/History")]
        public async Task<IActionResult> GetUserHistory(Guid id)
        {
            var query = new GetUserLessonHistory_Query(id);
            try
            {
                UserLessonHistory history = await _mediator.Send(query);
                return Ok(new UserLessonHistoryOutput(history));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }    
    }
}
