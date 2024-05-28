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

        public List<Lesson> GetAllStudentLesson(Guid userId)
        {
            return _database.Lessons
                .Where(lesson => lesson.Student != null && lesson.Student.Id == userId)
                .ToList();
        }

        public Lesson GetById(int id)
        {
            Lesson? lesson = _database.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson is null)
                throw new LessonNotFoundException();
            return lesson;
        }

        public List<Lesson> GetLessonsForUser(User user, DateTime start, DateTime end, bool onlyEmptyLesson = false)
        {
            IQueryable<Lesson> query = _database.Lessons;
            if (user.Type == UserType.Student)
                query = query.Where(lesson => lesson.Type == user.LicenceType);
            else if (user.Type == UserType.Teacher)
                query = query.Where(lesson => lesson.Teacher.Id == user.Id);


            if (onlyEmptyLesson)
                query = query.Where(lesson => lesson.Student == null);


            DateTime calculatedEndDate = end.AddDays(1).Date;
            return query
                .Where(lesson => lesson.Start >= start.Date && lesson.Start.AddMinutes(lesson.Duration.Value) <= calculatedEndDate)
                .ToList();
        }

        public List<Lesson> GetPassedLesson(Guid userId, DateTime now)
        {
            return _database.Lessons
                .Where(lesson => lesson.Student != null && lesson.Student.Id == userId && lesson.Start > now)
                .OrderBy(lesson => lesson.Start)
                .ToList();
        }

        public List<Lesson> GetUserHistory(Guid userId, DateTime now)
        {
            return _database.Lessons
                .Where(lesson => lesson.Student != null && lesson.Student.Id == userId && (lesson.Start.AddMinutes(lesson.Duration.Value)) <= now)
                .OrderByDescending(lesson => lesson.Start)
                .ToList();
        }

        public List<Lesson> GetUserPlanning(User user, DateTime start, DateTime end)
        {
            DateTime calculatedEndDate = end.Date.AddDays(1).Date;

            IQueryable<Lesson> query = _database.Lessons;
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
