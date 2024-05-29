using Domain.Models;
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
            return Task.FromResult(_lessonRepository.GetUserPlanning(user, request.Start, request.End));
        }
    }
}
