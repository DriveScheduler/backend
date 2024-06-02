using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Integration
{
    public class UserRepository : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly IDataAccessor _database;

        public UserRepository(SetupDependencies fixture)
        {
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
        }

        public void Dispose()
        {
            _database.Clear();
        }

        [Fact]
        public void UserShouldBeCreatedWithId()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);

            _userRepository.Insert(teacher);

            Assert.That(teacher.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Fact]
        public void UserShouldBeCreatedWithDifferentId()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);
            User student = new User("student", "student", "student@gmail.com", "mdp", LicenceType.Car, UserType.Student);
            User admin = new User("admin", "admin", "admin@gmail.com", "mdp", LicenceType.Car, UserType.Admin);

            _userRepository.Insert([teacher, student, admin]);

            Assert.That(teacher.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(student.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.Id, Is.Not.EqualTo(Guid.Empty));

            Assert.That(student.Id, Is.Not.EqualTo(teacher.Id));
            Assert.That(admin.Id, Is.Not.EqualTo(teacher.Id));
            Assert.That(admin.Id, Is.Not.EqualTo(student.Id));
        }

        [Fact]
        public void UserShouldBeGetByQuery()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);

            _userRepository.Insert(teacher);

            User userFromQuery = _userRepository.GetUserById(teacher.Id);
            Assert.NotNull(userFromQuery);
        }

        [Fact]
        public void UserShouldBeUpdate()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);

            _userRepository.Insert(teacher);

            User updatedTeacher = new User(teacher.Id, "teacher2", "teacher2", "teacher2@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);
            _userRepository.Update(updatedTeacher);

            User userFromQuery = _userRepository.GetUserById(teacher.Id);

            Assert.Equals(updatedTeacher, userFromQuery);
        }
    }
}
