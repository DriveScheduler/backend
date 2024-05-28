using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Models;
using Domain.Repositories;
using Domain.ValueObjects;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{    
    internal sealed class UserRepository(DatabaseContext database) : IUserRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<User>> GetAllTeachers()
        {
            return IncludeAllSubEntities()
                .Select(user => user.ToDomainModel())
                .ToListAsync();
        }      

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User_Database? user = await IncludeAllSubEntities().FirstOrDefaultAsync(user => user.Email == email);
            if (user is null)
                throw new UserNotFoundException();
            return user.ToDomainModel();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            User_Database? user = await IncludeAllSubEntities().FirstOrDefaultAsync(user => user.Id == id);
            if (user is null)
                throw new UserNotFoundException();
            return user.ToDomainModel();
        }

        public Guid Insert(User user)
        {
            User_Database dbUser = new(user);
            try
            {
                _database.Users.Add(dbUser);
                _database.SaveChanges();
                return dbUser.Id;
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }

        public List<Guid> Insert(List<User> users)
        {
            List<User_Database> dbUsers = users.Select(u => new User_Database(u)).ToList();
            try
            {
                _database.Users.AddRange(dbUsers);
                _database.SaveChanges();
                return dbUsers.Select(u => u.Id).ToList();
            }
            catch(Exception)
            {
                throw new UserSaveException();
            }
        }     

        public async Task UpdateAsync(User user)
        {
            User_Database? dbUuser = await IncludeAllSubEntities().FirstOrDefaultAsync(user => user.Id == user.Id);
            if (dbUuser is null)
                throw new UserNotFoundException();

            dbUuser.FromDomainModel(user);

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }    

        public async Task<Guid> InsertAsync(User user)
        {
            User_Database dbUser = new(user);
            try
            {
                _database.Users.Add(dbUser);
                await _database.SaveChangesAsync();
                return dbUser.Id;
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }

        private IQueryable<User_Database> IncludeAllSubEntities()
        {
            return _database.Users
                .Include(u => u.WaitingList)
                .Include(u => u.LessonsAsTeacher)
                .Include(u => u.LessonsAsStudent);         
        }      
    }
}
