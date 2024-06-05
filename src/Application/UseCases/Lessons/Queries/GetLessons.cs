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
                return Task.FromResult(GetLessonsForStudent(student, request.Start, request.End, request.TeacherIds, request.OnlyEmptyLesson));
            else if(user is Teacher teacher)
                return Task.FromResult(GetLessonsForTeacher(teacher, request.Start, request.End));
            else
                return Task.FromResult(new List<Lesson>());
        }

        private List<Lesson> GetLessonsForStudent(Student student, DateTime start, DateTime end, List<Guid> teacherIds, bool onlyEmptyLesson)
        {
            IEnumerable<Lesson> lessons = _lessonRepository.GetLessonsByLicenceType(student.LicenceType);
            
            if (teacherIds.Count > 0)
                lessons = lessons.Where(lesson => teacherIds.Contains(lesson.Teacher.Id));

            if (onlyEmptyLesson)
                lessons = lessons.Where(lesson => lesson.Student == null);

            DateTime calculatedEndDate = end.AddDays(1).Date;
            return lessons
                .Where(lesson => lesson.Start.Date >= start.Date.Date && lesson.End.Date <= calculatedEndDate.Date)
                .ToList();
        }

        private List<Lesson> GetLessonsForTeacher(Teacher teacher, DateTime start, DateTime end)
        {
            IEnumerable<Lesson> query = _lessonRepository.GetLessonsForTeacher(teacher);            

            DateTime calculatedEndDate = end.AddDays(1).Date;
            return query
                .Where(lesson => lesson.Start.Date >= start.Date.Date && lesson.Start.AddMinutes(lesson.Duration.Value).Date <= calculatedEndDate.Date)
                .ToList();
        }
    }
}
