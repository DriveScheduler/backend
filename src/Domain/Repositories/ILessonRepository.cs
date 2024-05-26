using Domain.Entities;

namespace Domain.Repositories
{
    public interface ILessonRepository
    {
        public int Insert(Lesson lesson);
        public Task<int> InsertAsync(Lesson lesson);
        public List<int> Insert(List<Lesson> lesson);
        public Task<List<int>> InsertAsync(List<Lesson> lesson);
        public Task UpdateAsync(Lesson lesson);
        public Task<Lesson> GetByIdAsync(int id);
        public Task<List<Lesson>> GetAllAsync();
        public Task<List<Lesson>> GetLessonsForUserAsync(Guid userId, DateTime start, DateTime end, bool onlyEmptyLesson=false);
        public Task<List<Lesson>> GetUserPlanning(Guid userId, DateTime start, DateTime end);
        public Task<List<Lesson>> GetLessonForUser(Guid userId, DateTime start, DateTime end);
        public Task<List<Lesson>> GetUserHistory(Guid userId);
        public Task<List<Lesson>> GetPassedLesson(Guid userId);
        public Task<List<Lesson>> GetAllStudentLesson(Guid userId);
    }
}
