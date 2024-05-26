using Domain.Entities;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    // FULL LOAD
    internal sealed class UserRepository(DatabaseContext database) : IUserRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<User> GetUserByIdAsync(Guid id)
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
