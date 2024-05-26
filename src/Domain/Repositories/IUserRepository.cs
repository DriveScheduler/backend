using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public Task<Guid> InsertAsync(User user);
        public Task UpdateAsync(User user);
        public Task<User> GetUserByIdAsync(Guid id);
        public Task<User> GetStudentById(Guid id);
        public Task<User> GetTeacherById(Guid id);        
        public Task<User> UpdateUserAsync(string email);
        public Task<List<User>> GetAllTeachers();
    }
}
