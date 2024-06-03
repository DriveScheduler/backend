using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using Infrastructure.Entities;
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

        public List<Lesson> GetLessonsForStudent(Student student, DateTime start, DateTime end, List<Guid> teacherIds, bool onlyEmptyLesson = false)
        {
            IEnumerable<Lesson> query = _database.Lessons;

            query = query.Where(lesson => lesson.Type == student.LicenceType);
            if (teacherIds.Count > 0)
                query = query.Where(lesson => teacherIds.Contains(lesson.Teacher.Id));

            if (onlyEmptyLesson)
                query = query.Where(lesson => lesson.Student == null);

            DateTime calculatedEndDate = end.AddDays(1).Date;
            return query
                .Where(lesson => lesson.Start.Date >= start.Date.Date && lesson.Start.AddMinutes(lesson.Duration.Value).Date <= calculatedEndDate.Date)
                .ToList();
        }

        public List<Lesson> GetLessonsForTeacher(Teacher teacher, DateTime start, DateTime end)
        {
            IEnumerable<Lesson> query = _database.Lessons;
            query = query.Where(lesson => lesson.Teacher.Id == teacher.Id);

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
            if (user.GetType() == typeof(Student))
                query = query
                    .Where(lesson => lesson.Student != null && lesson.Student.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);
            else if (user.GetType() == typeof(Teacher))
                query = query
                    .Where(lesson => lesson.Teacher.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);

            return query.ToList();
        }

        public int Insert(Lesson lesson)
        {
            try
            {
                LessonDataEntity lessonDataEntity = new LessonDataEntity(lesson);
                _database.Insert(lessonDataEntity);
                return lessonDataEntity.Id;
            }
            catch (Exception exc)
            {
                throw exc;
                //throw new LessonSaveException();
            }
        }

        public List<int> Insert(List<Lesson> lessons)
        {
            try
            {
                List<LessonDataEntity> lessonDataEntities = lessons.Select(l => new LessonDataEntity(l)).ToList();
                _database.Insert(lessonDataEntities);
                return lessonDataEntities.Select(l => l.Id).ToList();
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
                _database.Update(new LessonDataEntity(lesson));
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        public void Delete(Lesson lesson)
        {
            try
            {
                _database.Delete(new LessonDataEntity(lesson));
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }
    }
}
