using Domain.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public Guid Insert(User user);
        public List<Guid> Insert(List<User> users);
        public Task<Guid> InsertAsync(User user);
        public Task UpdateAsync(User user);
        public Task<User> GetUserByIdAsync(Guid id);        
        public Task<User> GetUserByEmailAsync(string email);        
        public Task<List<User>> GetAllTeachers();
    }
}
