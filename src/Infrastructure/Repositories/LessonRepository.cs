using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    internal sealed class LessonRepository(IDataAccessor database) : ILessonRepository
    {
        private readonly IDataAccessor _database = database;

        public List<Lesson> GetAll()
        {
            return _database.Lessons
                .ToList();
        }       

        public Lesson GetById(int id)
        {
            Lesson? lesson = _database.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson is null)
                throw new LessonNotFoundException();
            return lesson;
        }

        public List<Lesson> GetLessonsForUser(User user, DateTime start, DateTime end,List<Guid> teacherIds, bool onlyEmptyLesson = false)
        {
            IEnumerable<Lesson> query = _database.Lessons;
            if (user.Type == UserType.Student)
            {
                query = query.Where(lesson => lesson.Type == user.LicenceType);
                if (teacherIds.Count > 0)
                    query = query.Where(lesson => teacherIds.Contains(lesson.TeacherId));
            }
            else if (user.Type == UserType.Teacher)
                query = query.Where(lesson => lesson.Teacher.Id == user.Id);


            if (onlyEmptyLesson)
                query = query.Where(lesson => lesson.Student == null);


            DateTime calculatedEndDate = end.AddDays(1).Date;
            return query
                .Where(lesson => lesson.Start.Date >= start.Date.Date && lesson.Start.AddMinutes(lesson.Duration.Value).Date <= calculatedEndDate.Date)
                .ToList();
        }

        public List<Lesson> GetPassedLesson(User user, DateTime now)
        {
            return _database.Lessons
                .Where(lesson => lesson.Student == user && lesson.Start > now)
                .OrderBy(lesson => lesson.Start)
                .ToList();
        }

        public List<Lesson> GetUserHistory(User user, DateTime now)
        {
            return _database.Lessons
                .Where(lesson => lesson.Student == user && (lesson.Start.AddMinutes(lesson.Duration.Value)) <= now)
                .OrderByDescending(lesson => lesson.Start)
                .ToList();
        }

        public List<Lesson> GetUserPlanning(User user, DateTime start, DateTime end)
        {
            DateTime calculatedEndDate = end.Date.AddDays(1).Date;

            IEnumerable<Lesson> query = _database.Lessons;
            if (user.Type == UserType.Student)
                query = query
                    .Where(lesson => lesson.Student != null && lesson.Student.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);
            else if (user.Type == UserType.Teacher)
                query = query
                    .Where(lesson => lesson.Teacher.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);

            return query.ToList();
        }

        public void Insert(Lesson lesson)
        {
            try
            {
                _database.Insert(lesson);
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        public void Insert(List<Lesson> lessons)
        {
            try
            {
                _database.Insert(lessons);
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }


        public void Update(Lesson lesson)
        {
            try
            {
                _database.Update(lesson);
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }
    }
}
