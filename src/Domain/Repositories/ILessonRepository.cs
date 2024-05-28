using Domain.Models;

namespace Domain.Repositories
{
    public interface ILessonRepository
    {
        public int Insert(Lesson lesson);
        public Task<int> InsertAsync(Lesson lesson);
        public List<int> Insert(List<Lesson> lesson);        
        public Task UpdateAsync(Lesson lesson);
        public Task<Lesson> GetByIdAsync(int id);
        public Task<List<Lesson>> GetAllAsync();
        public Task<List<Lesson>> GetLessonsForUserAsync(User user, DateTime start, DateTime end, bool onlyEmptyLesson=false);
        public Task<List<Lesson>> GetUserPlanning(User user, DateTime start, DateTime end);                
        public Task<List<Lesson>> GetUserHistory(Guid userId, DateTime now);
        public Task<List<Lesson>> GetPassedLesson(Guid userId, DateTime now);
        public Task<List<Lesson>> GetAllStudentLesson(Guid userId);
    }
}
