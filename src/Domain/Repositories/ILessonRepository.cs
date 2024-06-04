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
    }
}
