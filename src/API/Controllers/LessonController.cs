using API.Inputs.Lessons;
using API.Inputs.Users;

using Application.UseCases.Lessons.Commands;
using Application.UseCases.Users.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LessonController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost]
        public async Task<IActionResult> Create(CreateLessonModel input)
        {
            var command = new CreateLesson_Command(input.Name, input.Start, input.Duration, input.TeacherId, input.Type, input.VehicleId, input.MaxStudent);
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

        [HttpPut]
        public async Task<IActionResult> Update(UpdateLessonModel input)
        {
            var command = new UpdateLesson_Command(input.Id, input.Name, input.Start, input.Duration, input.TeacherId, input.Type, input.VehicleId, input.MaxStudent);
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
    }
}
