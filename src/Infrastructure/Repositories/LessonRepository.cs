using Domain.Entities;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    // FULL LOAD
    internal sealed class LessonRepository(DatabaseContext database) : ILessonRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<Lesson>> GetAllAsync()
        {
            throw new NotImplementedException();
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
