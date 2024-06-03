using API.Authentication;
using API.Inputs.Lessons;
using API.Outputs.Lessons;

using Application.UseCases.Lessons.Commands;
using Application.UseCases.Lessons.Queries;
using Application.UseCases.Users.Queries;

using Domain.Enums;
using Domain.Models;
using Domain.Models.Users;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController(IMediator mediator) : JwtControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateLessonModel input)
        {
            var command = new CreateLesson_Command(input.Name, input.Start, input.Duration, GetUserId());
            try
            {
                int lessonId = await _mediator.Send(command);
                return Ok(lessonId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateLessonModel input)
        {
            var command = new UpdateLesson_Command(input.Id, input.Name, input.Start, input.Duration, GetUserId());
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int lessonId)
        {
            var command = new DeleteLesson_Command(lessonId, GetUserId());
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
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetLessonById_Query(id);
            try
            {
                Lesson lesson = await _mediator.Send(query);
                return Ok(new LessonLight(lesson));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Lessons")]
        public async Task<IActionResult> GetLessons([FromQuery]GetLessonModel input)
        {
            Guid userId = GetUserId();
            var query = new GetLessons_Query(userId, input.StartDate, input.EndDate, input.Teachers??new(), input.OnlyEmptyLesson);
            var userQuery = new GetUserById_Query(userId);
            try
            {
                List<Lesson> lessons = await _mediator.Send(query);
                User user = await _mediator.Send(userQuery);
                return Ok(lessons.Select(lesson => new LessonDetail(lesson, user)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AddStudentToLesson")]
        public async Task<IActionResult> AddStudentToLesson(UpdateLessonStudentModel input)
        {
            var query = new AddStudentToLesson_Command(input.LessonId, GetUserId());
            try
            {
                var t = User.Claims;
                await _mediator.Send(query);                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("RemoveStudentFromLesson")]
        public async Task<IActionResult> RemoveStudentFromLesson(UpdateLessonStudentModel input)
        {
            var query = new RemoveStudentFromLesson_Command(input.LessonId, GetUserId());
            try
            {
                await _mediator.Send(query);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AddStudentToWaitingList")]
        public async Task<IActionResult> AddStudentToWaitingList(UpdateLessonStudentModel input)
        {
            var query = new AddStudentToWaitingList_Command(input.LessonId, GetUserId());
            try
            {
                await _mediator.Send(query);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("RemoveStudentFromWaitingList")]
        public async Task<IActionResult> RemoveStudentFromWaitingList(UpdateLessonStudentModel input)
        {
            var query = new RemoveStudentFromWaitingList_Command(input.LessonId, GetUserId());
            try
            {
                await _mediator.Send(query);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
