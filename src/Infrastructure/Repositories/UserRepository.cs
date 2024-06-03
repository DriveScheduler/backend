using Domain.Exceptions.Users;
using Domain.Models.Users;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using System.Reflection;

namespace Infrastructure.Repositories
{
    public sealed class UserRepository(IDataAccessor database) : IUserRepository
    {
        private readonly IDataAccessor _database = database;

        public List<User> GetAllTeachers()
        {
            return _database.Users                
                .Where(user => user.GetType() == typeof(Teacher))
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
        public Teacher GetTeacherById(Guid id)
        {
            User user = GetUserById(id);
            if(user is Teacher teacher)            
                return teacher;        
            throw new UserNotInRoleException("L'utilisateur n'est pas un moniteur");
        }

        public Student GetStudentById(Guid id)
        {
            User user = GetUserById(id);
            if (user is Student student)
                return student;
            throw new UserNotInRoleException("L'utilisateur n'est pas un élève");
        }


        public bool IsEmailUnique(string email)
        {
            return _database.Users.FirstOrDefault(user => user.Email.Value == email) is null;
        }

        public Guid Insert(User user)
        {
            try
            {
                UserDataEntity userDataEntity = new UserDataEntity(user);
                _database.Insert(userDataEntity);
                SetPrivateField(user, nameof(User.Id), userDataEntity.Id);
                return userDataEntity.Id;
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }

        public List<Guid> Insert(List<User> users)
        {
            try
            {
                List<UserDataEntity> userDataEntities = users.Select(user => new UserDataEntity(user)).ToList();
                _database.Insert(userDataEntities);
                for (int i = 0; i < users.Count; i++)
                    SetPrivateField(users[i], nameof(User.Id), userDataEntities[i].Id);
                return userDataEntities.Select(userDataEntity => userDataEntity.Id).ToList();
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
                _database.Update(new UserDataEntity(user));
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            var field = typeof(T).GetField($"<{fieldName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(entity, value);
        }
    }
}
