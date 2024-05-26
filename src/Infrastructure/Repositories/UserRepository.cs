using Domain.Entities;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    // FULL LOAD
    internal sealed class UserRepository(DatabaseContext database) : IUserRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<User>> GetAllTeachers()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetStudentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetTeacherById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid Insert(User user)
        {
            throw new NotImplementedException();
        }

        public List<Guid> Insert(List<User> user)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<Guid> IUserRepository.InsertAsync(User user)
        {
            throw new NotImplementedException();
        }

        //public Task<int> Delete(int id)
        //{
        //    _database.Users.Remove(_database.Users.Find(id));
        //    return _database.SaveChangesAsync();
        //}

        //public Task<int> InsertAsync(User entity)
        //{
        //    _database.Users.Add(new User_Database(entity));
        //    return _database.SaveChangesAsync();
        //}

        //public Task<int> Update(User entity)
        //{
        //    User_Database? dbUser = _database.Users.Find(entity.Id);
        //    if (dbUser is null) return Task.FromResult(1);
        //    dbUser.FromDomainModel(entity);
        //    return _database.SaveChangesAsync();
        //}
    }
}
