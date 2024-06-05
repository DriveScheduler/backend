using Domain.Enums;
using Domain.Models.Users;
using Domain.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Integration
{
    public class UserRepository
    {
        private readonly IUserRepository _userRepository;        

        public UserRepository()
        {
            SetupDependencies fixture = new SetupDependencies();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();            
        }
        

        [Fact]
        public void UserShouldBeCreatedWithId()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);

            _userRepository.Insert(teacher);

            Assert.NotEqual(Guid.Empty, teacher.Id);
        }

        [Fact]
        public void UserShouldBeCreatedWithDifferentId()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);
            Student student = new Student("student", "student", "student@gmail.com", "mdp", LicenceType.Car);
            Admin admin = new Admin("admin", "admin", "admin@gmail.com", "mdp", LicenceType.Car);

            _userRepository.Insert([teacher, student, admin]);

            Assert.NotEqual(Guid.Empty, teacher.Id);
            Assert.NotEqual(Guid.Empty, student.Id);
            Assert.NotEqual(Guid.Empty, admin.Id);

            Assert.NotEqual(teacher.Id ,student.Id);
            Assert.NotEqual(teacher.Id, admin.Id);
            Assert.NotEqual(student.Id, admin.Id);
        }

        [Fact]
        public void UserShouldBeGetByQuery()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);

            _userRepository.Insert(teacher);

            User userFromQuery = _userRepository.GetUserById(teacher.Id);
            Assert.NotNull(userFromQuery);
        }

        [Fact]
        public void UserShouldBeUpdate()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);

            _userRepository.Insert(teacher);

            Teacher updatedTeacher = new Teacher(teacher.Id, "teacher2", "teacher2", "teacher2@gmail.com", "mdp", LicenceType.Car);
            _userRepository.Update(updatedTeacher);

            User userFromQuery = _userRepository.GetUserById(teacher.Id);

            Assert.Equal(updatedTeacher, userFromQuery);
        }
    }
}
