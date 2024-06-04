using Domain.Models.Users;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public void Insert(User user);
        public void Insert(List<User> users);        
        public void Update(User user);

        public User GetUserById(Guid id);
        public Teacher GetTeacherById(Guid id);
        public Student GetStudentById(Guid id);
        public User GetUserByEmail(string email);                  
        public List<Teacher> GetAllTeachers();

        public bool IsEmailUnique(string email);
    }
}
