using Domain.Models;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{    
    internal sealed class LessonRepository(DatabaseContext database) : ILessonRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<Lesson>> GetAllAsync()
        {
            return IncludeAllSubEntities()
                .Select(lesson => lesson.ToDomainModel())
                .ToListAsync();
        }

        public Task<List<Lesson>> GetAllStudentLesson(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Lesson> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetLessonForUser(Guid userId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetLessonsForUserAsync(Guid userId, DateTime start, DateTime end, bool onlyEmptyLesson = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetPassedLesson(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetUserHistory(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetUserPlanning(Guid userId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public int Insert(Lesson lesson)
        {
            throw new NotImplementedException();
        }

        public List<int> Insert(List<Lesson> lesson)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Lesson lesson)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> InsertAsync(List<Lesson> lesson)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Lesson lesson)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Lesson_Database> IncludeAllSubEntities()
        {
            return _database.Lessons
                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Student)
                .Include(lesson => lesson.Vehicle)
                .Include(lesson => lesson.WaitingList);
        }

        //public Task DeleteAsync(int id)
        //{
        //    _database.Lessons.Remove(_database.Lessons.Find(id));
        //    return _database.SaveChangesAsync();
        //}

        //public Task<Lesson> InsertAsync(Lesson entity)
        //{
        //    _database.Lessons.Add(new Lesson_Database(entity));
        //    return _database.SaveChangesAsync();
        //}

        //public Task UpdateAsync(Lesson entity)
        //{
        //    Lesson_Database? dbLesson = _database.Lessons.Find(entity.Id);
        //    if (dbLesson is null) return Task.FromResult(1);
        //    dbLesson.FromDomainModel(entity);
        //    return _database.SaveChangesAsync();
        //}
    }
}
