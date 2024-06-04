using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserPlanning_Query(Guid UserId, DateTime Start, DateTime End) : IRequest<List<Lesson>>;

    internal sealed class GetUserPlanning_QueryHandler(
        ILessonRepository lessonRepository,
        IUserRepository userRepository
        ) : IRequestHandler<GetUserPlanning_Query, List<Lesson>>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public Task<List<Lesson>> Handle(GetUserPlanning_Query request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserById(request.UserId);

            DateTime calculatedEndDate = request.End.Date.AddDays(1).Date;            
            List<Lesson> result;
            if (user is Student student)
            {
                result = _lessonRepository.GetLessonsForStudent(student)
                    .Where(lesson => lesson.Start >= request.Start && lesson.Start <= calculatedEndDate)
                    .ToList();
            }
            else if (user is Teacher teacher)
            {
                result = _lessonRepository.GetLessonsForTeacher(teacher)
                    .Where(lesson => lesson.Start >= request.Start && lesson.Start <= calculatedEndDate)
                    .ToList();
            }
            else result = [];             

            return Task.FromResult(result);
        }
    }
}
