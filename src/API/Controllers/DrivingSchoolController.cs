using API.Inputs.DrivingSchools;
using API.Outputs.DrivingSchools;

using Application.UseCases.DrivingSchools.Commands;
using Application.UseCases.DrivingSchools.Queries;
using Domain.Entities;
using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DrivingSchoolController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateDrivingSchoolModel input)
        {
            var command = new CreateDrivingSchool_Command(input.Name, input.Address);
            try
            {
                int id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateDrivingSchoolModel input)
        {
            var command = new UpdateDrivingSchool_Command(input.Id, input.Name, input.Address);
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
            var query = new GetDrivingSchoolById_Query(id);
            try
            {
                DrivingSchool drivingSchool = await _mediator.Send(query);
                return Ok(new DrivingSchoolLight(drivingSchool));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}