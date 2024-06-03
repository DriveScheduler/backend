using Domain.Models;

namespace Domain.Repositories
{
    public interface ILessonRepository
    {
        public void Insert(Lesson lesson);        
        public void Insert(List<Lesson> lesson);        
        public void Update(Lesson lesson);
        public void Delete(Lesson lesson);
        public Lesson GetById(int id);
        public List<Lesson> GetAll();
        public List<Lesson> GetStudentLessons(User user);
        public List<Lesson> GetLessonsForUser(User user, DateTime start, DateTime end, List<Guid> teacherIds, bool onlyEmptyLesson=false);        
        public List<Lesson> GetUserPlanning(User user, DateTime start, DateTime end);                
        public List<Lesson> GetUserHistory(User user, DateTime now);
        public List<Lesson> GetPassedLesson(User user, DateTime now);        
    }
}
