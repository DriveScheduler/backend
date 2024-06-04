using API.Authentication;
using API.Inputs.Users;
using API.Outputs.Users;

using Application.Models;
using Application.UseCases.Users.Commands;
using Application.UseCases.Users.Queries;
using Domain.Models.Users;
using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator, JwtProvider tokenProvider) : JwtControllerBase
    {

        private readonly IMediator _mediator = mediator;        
        private readonly JwtProvider _tokenProvider = tokenProvider;

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateUserModel input)
        {
            var command = new CreateUser_Command(input.Name, input.FirstName, input.Email, input.Password, input.LicenceType, input.Type);
            try
            {
                Guid userId = await _mediator.Send(command);
                string token = _tokenProvider.GenerateToken(userId, input.FirstName, input.Type);
                return Ok(new UserAuthenticated() { UserId = userId, Token = token});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateUserModel input)
        {
            var command = new UpdateUser_Command(GetUserId(), input.Name, input.FirstName, input.Email);
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetUserById_Query(GetUserId());
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
                List<Teacher> teachers = await _mediator.Send(query);
                return Ok(teachers.Select(teacher => new UserLight(teacher)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetUserDashboard()
        {
            var query = new GetUserDashboard_Query(GetUserId());
            try
            {
                UserDashboard dashboard = await _mediator.Send(query);
                User connectedUser = await _mediator.Send(new GetUserById_Query(GetUserId()));
                return Ok(new UserDashboardOutput(dashboard, connectedUser));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }            
        }

        [HttpGet("Planning")]
        public async Task<IActionResult> GetUserPlanning()
        {
            var query = new GetUserLessonPlanning_Query(GetUserId());
            try
            {
                UserLessonPlanning planning = await _mediator.Send(query);
                User connectedUser = await _mediator.Send(new GetUserById_Query(GetUserId()));
                return Ok(new UserLessonPlanningOutput(planning, connectedUser));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetUserHistory()
        {
            var query = new GetUserLessonHistory_Query(GetUserId());
            try
            {
                UserLessonHistory history = await _mediator.Send(query);
                User connectedUser = await _mediator.Send(new GetUserById_Query(GetUserId()));
                return Ok(new UserLessonHistoryOutput(history, connectedUser));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LogInUserModel model)
        {
            var query = new LogIn_Query(model.Email, model.Password);
            try
            {
                User user = await _mediator.Send(query);
                string token = _tokenProvider.GenerateToken(user.Id, user.FirstName.Value, user.GetRole());
                return Ok(new UserAuthenticated() { UserId = user.Id, Token = token});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
