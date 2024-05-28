﻿using Domain.Models;
using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Queries
{
    public sealed record GetLessons_Query(Guid UserId, DateTime Start, DateTime End, bool OnlyEmptyLesson=false) : IRequest<List<Lesson>>;

    internal sealed class GetLessons_QueryHandler(
        ILessonRepository lessonRepository,
        IUserRepository userRepository
        ) : IRequestHandler<GetLessons_Query, List<Lesson>>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public Task<List<Lesson>> Handle(GetLessons_Query request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserById(request.UserId);
            return Task.FromResult(_lessonRepository.GetLessonsForUser(user, request.Start, request.End, request.OnlyEmptyLesson));

            //IQueryable<Lesson> query = _database.Lessons
            //    .Include(l => l.Student)
            //    .Include(l => l.WaitingList)
            //    .Include(l => l.Teacher)
            //    .Include(l => l.Vehicle);

            //User? user = await _database.Users.FindAsync(request.UserId);
            //if (user is null)
            //    throw new UserNotFoundException();
            //if (user.Type == UserType.Student)                         
            //    query = query.Where(lesson => lesson.Type == user.LicenceType);            
            //else if (user.Type == UserType.Teacher)
            //    query = query.Where(lesson => lesson.Teacher == user);       
            

            //if (request.OnlyEmptyLesson.HasValue && request.OnlyEmptyLesson.Value)            
            //    query = query.Where(lesson => lesson.Student == null);            
            

            //DateTime calculatedEndDate = request.End.AddDays(1).Date;
            //return await query
            //    .Where(lesson => lesson.Start >= request.Start.Date && lesson.Start.AddMinutes(lesson.Duration) <= calculatedEndDate)
            //    .ToListAsync();
        }
    }
}
