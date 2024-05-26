using Domain.Entities;

namespace Domain.Repositories
{
    public interface ILessonRepository
    {
        public Task<int> InsertAsync(Lesson lesson);
        public Task UpdateAsync(Lesson lesson);
        public Task<Lesson> GetByIdAsync(int id);
        public Task<List<Lesson>> GetAllAsync();
        public Task<List<Lesson>> GetLessonsForUserAsync(Guid userId, DateTime start, DateTime end, bool onlyEmptyLesson=false);
    }
}
