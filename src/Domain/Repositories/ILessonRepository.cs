using Domain.Enums;
using Domain.Models;
using Domain.Models.Users;

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
        public List<Lesson> GetLessonForUser(User user);
        public List<Lesson> GetLessonsForStudent(Student student);
        public List<Lesson> GetWaitingLessonsForStudent(Student student);
        public List<Lesson> GetLessonsForTeacher(Teacher teacher);
        public List<Lesson> GetLessonsByLicenceType(LicenceType licenceType);

        //public List<Lesson> GetLessonsForStudent(Student student, DateTime start, DateTime end, List<Guid> teacherIds, bool onlyEmptyLesson=false);        
        //public List<Lesson> GetLessonsForTeacher(Teacher teacher, DateTime start, DateTime end);        
        //public List<Lesson> GetUserPlanning(User user, DateTime start, DateTime end);                
        //public List<Lesson> GetUserHistory(User user, DateTime now);
        //public List<Lesson> GetPassedLesson(User user, DateTime now);        
    }
}
