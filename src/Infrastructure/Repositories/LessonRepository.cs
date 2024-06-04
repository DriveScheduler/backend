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

        public void Insert(Lesson lesson)
        {
            try
            {
                LessonDataEntity lessonDataEntity = new LessonDataEntity(lesson);
                _database.Add(lessonDataEntity);
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
                _database.AddRange(lessonDataEntities);
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
                LessonDataEntity? dataEntity = _database.Lessons.FirstOrDefault(l => l.Id == lesson.Id);
                if (dataEntity is null)
                    throw new LessonNotFoundException();
                dataEntity.FromDomainModel(lesson);
                _database.SaveChanges();                
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
                LessonDataEntity? dataEntity = _database.Lessons.FirstOrDefault(l => l.Id == lesson.Id);
                if (dataEntity is null)
                    throw new LessonNotFoundException();              
                _database.Remove(dataEntity);
                _database.SaveChanges();                
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
            LessonDataEntity? dataEntity = _database.Lessons.FirstOrDefault(l => l.Id == id);
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
