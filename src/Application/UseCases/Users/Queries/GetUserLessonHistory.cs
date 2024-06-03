using Application.Abstractions;
using Application.Models;

using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetUserLessonHistory_Query(Guid UserId) : IRequest<UserLessonHistory>;

    internal sealed class GetUserLessonHistory_QueryHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        ISystemClock clock
        ) : IRequestHandler<GetUserLessonHistory_Query, UserLessonHistory>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _clock = clock;

        public Task<UserLessonHistory> Handle(GetUserLessonHistory_Query request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetUserById(request.UserId);
            List<Lesson> lessons = _lessonRepository.GetUserHistory(user, _clock.Now);

            UserLessonHistory history = new UserLessonHistory()
            {
                Lessons = lessons,
                LessonTotalTime = lessons.Sum(lesson => lesson.Duration.Value)
            };
            return Task.FromResult(history);
        }
    }
}
