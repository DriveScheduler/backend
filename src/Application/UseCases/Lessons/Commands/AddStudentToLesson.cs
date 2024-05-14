﻿using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Users;
using Domain.Validators.Lessons;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record AddStudentToLesson_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class AddStudentToLesson_CommandHandler(IDatabase database, ISystemClock systemClock) : IRequestHandler<AddStudentToLesson_Command>
    {
        private readonly IDatabase _database = database;
        private readonly ISystemClock _systemClock = systemClock;

        public async Task Handle(AddStudentToLesson_Command request, CancellationToken cancellationToken)
        {
            User? student = _database.Users.Find(request.UserId);
            if (student is null)
                throw new UserNotFoundException();

            Lesson? lesson = _database.Lessons
                .Include(Lesson => Lesson.Student)
                .FirstOrDefault(l => l.Id == request.LessonId);
            if (lesson is null)
                throw new LessonNotFoundException();

            LessonValidator validator = new LessonValidator(lesson, _systemClock)
                .AddStudentRules();

            lesson.Student = student;
            validator.ThrowIfInvalid(lesson);

            if (await _database.SaveChangesAsync() != 1)
                throw new LessonSaveException();
        }
    }
}

