using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;
using Domain.Repositories;

using Microsoft.Extensions.DependencyInjection;


namespace Integration
{
    public class LessonRepository
    {
        private readonly ILessonRepository _lessonRepository;        
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public LessonRepository()
        {
            SetupDependencies fixture = new SetupDependencies();            

            _lessonRepository = fixture.ServiceProvider.GetRequiredService<ILessonRepository>();
            _userRepository = fixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _vehicleRepository = fixture.ServiceProvider.GetRequiredService<IVehicleRepository>();
        }  


        [Fact]
        public void LessonShouldBeCreatedWithId()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);
            Car vehicle = new Car("AA123AA", "Renault");
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(vehicle);
            _lessonRepository.Insert(lesson);

            Assert.NotEqual((int)default, lesson.Id);
        }

        [Fact]
        public void LessonShouldBeGetByQuery()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);
            Car vehicle = new Car("AA123AA", "Renault");
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(vehicle);
            _lessonRepository.Insert(lesson);

            Lesson lessonFromQuery = _lessonRepository.GetById(lesson.Id);
            Assert.NotNull(lessonFromQuery);
        }

        [Fact]
        public void LessonShouldBeUpdate()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);
            Car vehicle = new Car("AA123AA", "Renault");
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(vehicle);
            _lessonRepository.Insert(lesson);

            Lesson updatedLesson = new Lesson(lesson.Id, "Cours 2", DateTime.Now.AddDays(1), 120, teacher, LicenceType.Car, vehicle);
            _lessonRepository.Update(updatedLesson);

            Lesson lessonFromQuery = _lessonRepository.GetById(lesson.Id);

            Assert.Equal(updatedLesson, lessonFromQuery);
        }

        [Fact]
        public void LessonShouldBeDelete()
        {
            Teacher teacher = new Teacher("teacher", "teacher", "teacher@gmail.com", "mdp", LicenceType.Car);
            Car vehicle = new Car("AA123AA", "Renault");
            Lesson lesson = new Lesson("Cours 1", DateTime.Now, 30, teacher, LicenceType.Car, vehicle);

            _userRepository.Insert(teacher);
            _vehicleRepository.Insert(vehicle);
            _lessonRepository.Insert(lesson);

            _lessonRepository.Delete(lesson);

            Assert.Throws<LessonNotFoundException>(() => _lessonRepository.GetById(lesson.Id));
        }
    }
}