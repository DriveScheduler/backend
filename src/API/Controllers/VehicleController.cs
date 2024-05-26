using API.Inputs.Vehicles;
using API.Outputs.Vehicles;

using Application.UseCases.Vehicles.Commands;
using Application.UseCases.Vehicles.Queries;
using Domain.Entities;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateVehicleModel input)
        {
            var command = new CreateVehicle_Command(input.RegistrationNumber, input.Name, input.Type);
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
        public async Task<IActionResult> Update(UpdateVehicleModel input)
        {
            var command = new UpdateVehicle_Command(input.Id, input.RegistrationNumber, input.Name, input.Type);
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
            var query = new GetVehicleById_Query(id);
            try
            {
                Vehicle vehicle = await _mediator.Send(query);
                return Ok(new VehicleLight(vehicle));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
