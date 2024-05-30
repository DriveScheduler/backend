using Domain.Enums;
using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Queries
{
    public sealed record GetLessons_Query(Guid UserId, DateTime Start, DateTime End, List<Guid> TeacherIds, bool OnlyEmptyLesson = false) : IRequest<List<Lesson>>;

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
            return Task.FromResult(_lessonRepository.GetLessonsForUser(user, request.Start, request.End, request.TeacherIds, request.OnlyEmptyLesson));
        }
    }
}
