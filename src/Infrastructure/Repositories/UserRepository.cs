using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    internal sealed class UserRepository(IDataAccessor database) : IUserRepository
    {
        private readonly IDataAccessor _database = database;

        public List<User> GetAllTeachers()
        {
            return _database.Users
                .Where(user => user.Type == UserType.Teacher)
                .ToList();
        }

        public User GetUserByEmail(string email)
        {
            User? user = _database.Users.FirstOrDefault(user => user.Email.Value == email);
            if (user is null)
                throw new UserNotFoundException();
            return user;
        }

        public User GetUserById(Guid id)
        {
            User? user = _database.Users.FirstOrDefault(user => user.Id == id);
            if (user is null)
                throw new UserNotFoundException();
            return user;
        }

        public bool IsEmailUnique(string email)
        {
            return _database.Users.FirstOrDefault(user => user.Email.Value == email) is null;
        }

        public void Insert(User user)
        {
            try
            {
                _database.Insert(user);
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }

        public void Insert(List<User> users)
        {
            try
            {
                _database.Insert(users);
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }        

        public void Update(User user)
        {
            try
            {
                _database.Update(user);
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }
    }
}
