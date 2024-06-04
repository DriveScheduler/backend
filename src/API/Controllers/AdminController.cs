using API.Authentication;
using API.Inputs.Vehicles;
using API.Outputs.Users;
using API.Outputs.Vehicles;
using Application.UseCases.Users.Commands;
using Application.UseCases.Users.Queries;
using Application.UseCases.Vehicles.Commands;
using Application.UseCases.Vehicles.Queries;

using Domain.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    public class AdminController(IMediator mediator) : JwtControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateVehicle")]
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

        [HttpPut("UpdateVehicle")]
        public async Task<IActionResult> Update(UpdateVehicleModel input)
        {
            var command = new UpdateVehicle_Command(input.Id, input.RegistrationNumber, input.Name);
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

        [HttpGet("Vehicles/{id}")]
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
        
        [HttpDelete("Vehicles/{id}")]
        public async Task<IActionResult> DeleteVehicleById(int id)
        {
            var command = new DeleteVehicle_Command(id);
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

        [HttpGet("Vehicles")]
        public async Task<IActionResult> Vehicles()
        {
            var query = new GetAllVehicles_Query();
            try
            {
                List<Vehicle> vehicle = await _mediator.Send(query);
                return Ok(vehicle.Select(v => new VehicleLight(v)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            var query = new GetAllUsers_Query();
            try
            {
                List<User> users = await _mediator.Send(query);
                return Ok(users.Select(u => new UserLight(u)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            var command = new DeleteUser_Command(id);
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
