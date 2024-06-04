using Domain.Exceptions.Users;
using Domain.Models.Users;
using Domain.Repositories;

using System.Reflection;

namespace UseCases.Fakes.Repositories
{
    internal sealed class FakeUserRepository : IUserRepository
    {
        private List<User> _users = [];

        public void Clear()
        {
            _users = [];
        }

        public List<Teacher> GetAllTeachers()
        {
            return _users.OfType<Teacher>().ToList();
        }

        public Student GetStudentById(Guid id)
        {
            User user = GetUserById(id);
            if (user is Student student)
                return student;
            throw new UserIsNotAStudentException();
        }

        public Teacher GetTeacherById(Guid id)
        {
            User user = GetUserById(id);
            if (user is Teacher teacher)
                return teacher;
            throw new UserIsNotATeacherException();

        }

        public User GetUserByEmail(string email)
        {
            User? user = _users.FirstOrDefault(u => u.Email.Value == email);
            if (user is null)
                throw new UserNotFoundException();
            return user;
        }

        public User GetUserById(Guid id)
        {
            User? user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                throw new UserNotFoundException();
            return user;
        }

        public void Insert(User user)
        {
            if (user.Id == Guid.Empty)
            {
                Guid id = new Guid();
                SetPrivateField(user, nameof(user.Id), id);
            }
            _users.Add(user);
        }

        public void Insert(List<User> users)
        {
            foreach (var user in users)
            {
                if (user.Id == Guid.Empty)
                {
                    SetPrivateField(user, nameof(user.Id), new Guid());
                }
            }
            _users.AddRange(users);
        }

        public bool IsEmailUnique(string email)
        {
            return _users.FirstOrDefault(u => u.Email.Value == email) is null;
        }

        public void Update(User user)
        {
        }

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            typeof(T)
              .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(entity, value);
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public void DeleteById(Guid id)
        {
            User user = GetUserById(id);
            _users.Remove(user);
        }
    }
}
