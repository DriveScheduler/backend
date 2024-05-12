﻿using API.Inputs.Lessons;

using Application.UseCases.Lessons.Commands;
using Application.UseCases.Users.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LessonController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateLessonModel input)
        {
            var command = new CreateLesson_Command(input.Name, input.Start, input.Duration, input.TeacherId, input.Type);
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
            var command = new UpdateLesson_Command(input.Id, input.Name, input.Start, input.Duration, input.TeacherId, input.Type, input.VehicleId);
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
                return Ok(lesson);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AddStudentToLesson")]
        public async Task<IActionResult> AddStudentToLesson(UpdateLessonStudentModel input)
        {
            var query = new AddStudentToLesson_Command(input.LessonId, input.StudentId);
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

        [HttpPut("RemoveStudentFromLesson")]
        public async Task<IActionResult> RemoveStudentFromLesson(UpdateLessonStudentModel input)
        {
            var query = new RemoveStudentFromLesson_Command(input.LessonId, input.StudentId);
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
            var query = new AddStudentToWaitingList_Command(input.LessonId, input.StudentId);
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
            var query = new RemoveStudentFromWaitingList_Command(input.LessonId, input.StudentId);
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