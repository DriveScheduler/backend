using Domain.Models;
using Domain.Models.Users;
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
            if(user is Student student)
                return Task.FromResult(_lessonRepository.GetLessonsForStudent(student, request.Start, request.End, request.TeacherIds, request.OnlyEmptyLesson));
            else if(user is Teacher teacher)
                return Task.FromResult(_lessonRepository.GetLessonsForTeacher(teacher, request.Start, request.End));
            else
                return Task.FromResult(new List<Lesson>());
        }
    }
}
