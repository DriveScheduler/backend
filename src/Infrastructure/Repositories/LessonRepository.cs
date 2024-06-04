using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using System.Reflection;

namespace Infrastructure.Repositories
{
    internal sealed class LessonRepository(DatabaseContext database) : ILessonRepository
    {
        private readonly DatabaseContext _database = database;

        //public List<Lesson> GetAll()
        //{
        //    return _database.Lessons
        //        .Select(l => l.FullDomainModel())
        //        .ToList();
        //}

        //public Lesson GetById(int id)
        //{
        //    LessonDataEntity? lesson = _database.Lessons.Find(id);
        //    if (lesson is null)
        //        throw new LessonNotFoundException();
        //    return lesson.FullDomainModel();
        //}

        //public List<Lesson> GetLessonsForStudent(Student student, DateTime start, DateTime end, List<Guid> teacherIds, bool onlyEmptyLesson = false)
        //{
        //    IEnumerable<Lesson> query = _database.Lessons.Select(l => l.FullDomainModel());

        //    query = query.Where(lesson => lesson.Type == student.LicenceType);
        //    if (teacherIds.Count > 0)
        //        query = query.Where(lesson => teacherIds.Contains(lesson.Teacher.Id));

        //    if (onlyEmptyLesson)
        //        query = query.Where(lesson => lesson.Student == null);

        //    DateTime calculatedEndDate = end.AddDays(1).Date;
        //    return query
        //        .Where(lesson => lesson.Start.Date >= start.Date.Date && lesson.Start.AddMinutes(lesson.Duration.Value).Date <= calculatedEndDate.Date)
        //        .ToList();
        //}

        //public List<Lesson> GetLessonsForTeacher(Teacher teacher, DateTime start, DateTime end)
        //{
        //    IEnumerable<Lesson> query = _database.Lessons.Select(l => l.FullDomainModel());
        //    query = query.Where(lesson => lesson.Teacher.Id == teacher.Id);

        //    DateTime calculatedEndDate = end.AddDays(1).Date;
        //    return query
        //        .Where(lesson => lesson.Start.Date >= start.Date.Date && lesson.Start.AddMinutes(lesson.Duration.Value).Date <= calculatedEndDate.Date)
        //        .ToList();
        //}

        //public List<Lesson> GetPassedLesson(User user, DateTime now)
        //{
        //    return _database.Lessons
        //        .Where(lesson => lesson.StudentId == user.Id && lesson.Start > now)
        //        .OrderBy(lesson => lesson.Start)
        //        .Select(l => l.FullDomainModel())
        //        .ToList();
        //}

        //public List<Lesson> GetUserHistory(User user, DateTime now)
        //{
        //    return _database.Lessons
        //        .Select(l => l.FullDomainModel())
        //        .Where(lesson => lesson.Student == user && (lesson.Start.AddMinutes(lesson.Duration.Value)) <= now)
        //        .OrderByDescending(lesson => lesson.Start)
        //        .ToList();
        //}

        //public List<Lesson> GetUserPlanning(User user, DateTime start, DateTime end)
        //{
        //    DateTime calculatedEndDate = end.Date.AddDays(1).Date;

        //    IEnumerable<Lesson> query = _database.Lessons.Select(l => l.FullDomainModel());
        //    if (user.GetType() == typeof(Student))
        //        query = query
        //            .Where(lesson => lesson.Student != null && lesson.Student.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);
        //    else if (user.GetType() == typeof(Teacher))
        //        query = query
        //            .Where(lesson => lesson.Teacher.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);

        //    return query.ToList();
        //}

        public void Insert(Lesson lesson)
        {
            try
            {
                LessonDataEntity lessonDataEntity = new LessonDataEntity(lesson);
                _database.Lessons.Add(lessonDataEntity);
                _database.SaveChanges();
                SetPrivateField(lesson, nameof(Lesson.Id), lessonDataEntity.Id);                
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
                List<LessonDataEntity> lessonDataEntities = lessons.Select(l => new LessonDataEntity(l)).ToList();
                _database.Lessons.AddRange(lessonDataEntities);
                _database.SaveChanges();
                for (int i = 0; i < lessons.Count; i++)
                    SetPrivateField(lessons[i], nameof(Lesson.Id), lessonDataEntities[i].Id);               
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
                LessonDataEntity? dataEntity = _database.Lessons.Find(lesson.Id);
                if (dataEntity is null)
                    throw new LessonNotFoundException();
                dataEntity.FromDomainModel(lesson);
                _database.SaveChanges();
                //_database.Update(dataEntity);
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
                LessonDataEntity? dataEntity = _database.Lessons.Find(lesson.Id);
                if (dataEntity is null)
                    throw new LessonNotFoundException();              
                _database.Lessons.Remove(dataEntity);
                _database.SaveChanges();
                //_database.Delete(dataEntity);
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            typeof(T)
              .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(entity, value);
        }

        public Lesson GetById(int id)
        {
            LessonDataEntity? dataEntity = _database.Lessons.Find(id);
            if (dataEntity is null)
                throw new LessonNotFoundException();
            return dataEntity.FullDomainModel();
        }

        public List<Lesson> GetAll()
        {
            return _database.Lessons
                .Select(l => l.FullDomainModel())
                .ToList();
        }

        public List<Lesson> GetLessonForUser(User user)
        {
            return _database.Lessons
                .Where(l => l.StudentId == user.Id || l.TeacherId == user.Id)
                .Select(l => l.FullDomainModel())
                .ToList();
        }

        public List<Lesson> GetLessonsForStudent(Student student)
        {
            return _database.Lessons
                .Where(l => l.StudentId == student.Id)
                .Select(l => l.FullDomainModel())
                .ToList();
        }

        public List<Lesson> GetWaitingLessonsForStudent(Student student)
        {
            return _database.Lessons
                .Where(l => l.UserWaitingLists.Select(u => u.UserId).Contains(student.Id))
                .Select(l => l.FullDomainModel())
                .ToList();
        }

        public List<Lesson> GetLessonsForTeacher(Teacher teacher)
        {
            return _database.Lessons
               .Where(l => l.TeacherId == teacher.Id)
               .Select(l => l.FullDomainModel())
               .ToList();
        }

        public List<Lesson> GetLessonsByLicenceType(LicenceType licenceType)
        {
            return _database.Lessons
                .Where(l => l.Type == licenceType)
                .Select(l => l.FullDomainModel())
                .ToList();
        }      
    }
}
