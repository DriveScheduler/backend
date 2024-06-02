using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Integration
{
    public class LessonRepository : IClassFixture<SetupDependencies>, IDisposable
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IDataAccessor _database;


        public LessonRepository(SetupDependencies fixture)
        {
            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _database = fixture.ServiceProvider.GetRequiredService<IDataAccessor>();
        }

        public void Dispose()
        {
            _database.Clear();
        }


        [Fact]
        public void LessonShouldBeCreatedWithId()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);
            Vehicle vehicle = new Vehicle("AA-123-AA", "Renault", LicenceType.Car);
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _lessonRepository.Insert(lesson);

            Assert.That(lesson.Id, Is.Not.EqualTo((int)default));
        }

        [Fact]
        public void LessonShouldBeGetByQuery()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);
            Vehicle vehicle = new Vehicle("AA-123-AA", "Renault", LicenceType.Car);
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _lessonRepository.Insert(lesson);

            Lesson lessonFromQuery = _lessonRepository.GetById(lesson.Id);
            Assert.NotNull(lessonFromQuery);
        }

        [Fact]
        public void LessonShouldBeUpdate()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);
            Vehicle vehicle = new Vehicle("AA-123-AA", "Renault", LicenceType.Car);
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _lessonRepository.Insert(lesson);

            Lesson updatedLesson = new Lesson(lesson.Id, "Cours 2", DateTime.Now.AddDays(1), 120, teacher, LicenceType.Car, vehicle);
            _lessonRepository.Update(updatedLesson);

            Lesson lessonFromQuery = _lessonRepository.GetById(lesson.Id);

            Assert.Equals(updatedLesson, lessonFromQuery);
        }

        [Fact]
        public void LessonShouldBeDelete()
        {
            User teacher = new User("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car, UserType.Teacher);
            Vehicle vehicle = new Vehicle("AA-123-AA", "Renault", LicenceType.Car);
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _lessonRepository.Insert(lesson);

            _lessonRepository.Delete(lesson);

            Assert.Throws<LessonNotFoundException>(() => _lessonRepository.GetById(lesson.Id));
        }
    }
}