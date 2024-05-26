using Domain.Entities;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserPlanning_Query(Guid UserId, DateTime Start, DateTime End) : IRequest<List<Lesson>>;

    internal sealed class GetUserPlanning_QueryHandler(ILessonRepository lessonRepository) : IRequestHandler<GetUserPlanning_Query, List<Lesson>>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;

        public async Task<List<Lesson>> Handle(GetUserPlanning_Query request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.GetUserPlanning(request.UserId, request.Start, request.End);
            //User? user = await _database.Users.FindAsync(request.UserId);
            //if (user is null)
            //    throw new UserNotFoundException();

            //DateTime calculatedEndDate = request.End.Date.AddDays(1).Date;

            //IQueryable<Lesson> query = _database.Lessons
            //        .Include(lesson => lesson.Student)
            //        .Include(lesson => lesson.Teacher)
            //        .Include(lesson => lesson.Vehicle);

            //if (user.Type == UserType.Student)
            //    return await query
            //        .Where(lesson => lesson.Student == user && lesson.Start >= request.Start && lesson.Start <= calculatedEndDate)
            //        .ToListAsync();
            //else if(user.Type == UserType.Teacher)
            //    return await query
            //        .Where(lesson => lesson.Teacher == user && lesson.Start >= request.Start && lesson.Start <= calculatedEndDate)
            //        .ToListAsync();
            //else
            //    return new List<Lesson>();
        }
    }
}
