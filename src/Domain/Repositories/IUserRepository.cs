using Domain.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public void Insert(User user);
        public void Insert(List<User> users);        
        public void Update(User user);
        public User GetUserById(Guid id);        
        public User GetUserByEmail(string email);        
        public List<User> GetAll();
        public List<User> GetAllTeachers();
        public bool IsEmailUnique(string email);
    }
}
