using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.Models.Users;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using System.Reflection;

namespace Infrastructure.Repositories
{
    internal sealed class UserRepository(DatabaseContext database) : IUserRepository
    {
        private readonly DatabaseContext _database = database;

        #region GET
        public void Insert(User user)
        {
            try
            {
                UserDataEntity userDataEntity = new UserDataEntity(user);
                _database.Add(userDataEntity);
                _database.SaveChanges();
                SetPrivateField(user, nameof(User.Id), userDataEntity.Id);
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
                List<UserDataEntity> userDataEntities = users.Select(user => new UserDataEntity(user)).ToList();
                _database.AddRange(userDataEntities);
                _database.SaveChanges();
                for (int i = 0; i < users.Count; i++)
                    SetPrivateField(users[i], nameof(User.Id), userDataEntities[i].Id);
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
                UserDataEntity? dataEntity = _database.Users.FirstOrDefault(u => u.Id == user.Id);
                if (dataEntity is null)
                    throw new UserNotFoundException();
                dataEntity.FromDomainModel(user);
                _database.SaveChanges();
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }

        #endregion

        #region UPDATE
        public List<Teacher> GetAllTeachers()
        {
            return _database.Users
                .Where(u => u.Type == UserType.Teacher)
                .Select(u => (Teacher)u.FullDomainModel())
                .ToList();
        }

        public User GetUserById(Guid id)
        {
            UserDataEntity? user = _database.Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new UserNotFoundException();
            return user.FullDomainModel();
        }

        public Teacher GetTeacherById(Guid id)
        {
            User user = GetUserById(id);
            if (user is Teacher teacher)
                return teacher;
            throw new UserIsNotATeacherException();
        }

        public Student GetStudentById(Guid id)
        {
            User user = GetUserById(id);
            if (user is Student student)
                return student;
            throw new UserIsNotAStudentException();
        }

        public User GetUserByEmail(string email)
        {
            UserDataEntity? user = _database.Users.FirstOrDefault(u => u.Email == email);
            if (user is null)
                throw new UserNotFoundException();
            return user.FullDomainModel();
        }

        public bool IsEmailUnique(string email)
        {
            return _database.Users.FirstOrDefault(u => u.Email == email) is null;
        }
        #endregion

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            var field = typeof(T).GetField($"<{fieldName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(entity, value);
        }       
    }
}
