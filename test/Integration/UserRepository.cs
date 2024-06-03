﻿using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models.Users;
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
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);

            _userRepository.Insert(teacher);

            Assert.That(teacher.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Fact]
        public void UserShouldBeCreatedWithDifferentId()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);
            Student student = new Student("student", "student", "student@gmail.com", "mdp", LicenceType.Car);
            Admin admin = new Admin("admin", "admin", "admin@gmail.com", "mdp", LicenceType.Car);

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

            Assert.Equals(updatedTeacher, userFromQuery);
        }
    }
}
