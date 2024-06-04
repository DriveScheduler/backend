using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Models.Users;
using Domain.Repositories;

using System.Reflection;

namespace UseCases.Fakes.Repositories
{
    internal sealed class FakeLessonRepository : ILessonRepository
    {
        private List<Lesson> _lessons = [];

        public void Clear()
        {
            _lessons = [];
        }

        public void Delete(Lesson lesson)
        {
            _lessons.Remove(lesson);
        }

        public List<Lesson> GetAll()
        {
            return _lessons;
        }

        public Lesson GetById(int id)
        {
            Lesson? lesson = _lessons.FirstOrDefault(l => l.Id == id);
            if (lesson is null) throw new LessonNotFoundException();
            return lesson;
        }

        public List<Lesson> GetLessonForUser(User user)
        {
            return _lessons.Where(l => l.Student?.Id == user.Id || l.Teacher.Id == user.Id).ToList();
        }

        public List<Lesson> GetLessonsByLicenceType(LicenceType licenceType)
        {
            return _lessons.Where(l => l.Type == licenceType).ToList();
        }

        public List<Lesson> GetLessonsForStudent(Student student)
        {
            return _lessons.Where(l => l.Student?.Id == student.Id).ToList();
        }

        public List<Lesson> GetLessonsForTeacher(Teacher teacher)
        {
            return _lessons.Where(l => l.Teacher.Id == teacher.Id).ToList();
        }

        public List<Lesson> GetWaitingLessonsForStudent(Student student)
        {
            return _lessons.Where(l => l.WaitingList.Select(u => u.Id).Contains(student.Id)).ToList();
        }

        public void Insert(Lesson lesson)
        {
            if (lesson.Id is default(int))
            {
                int id = _lessons.Count == 0 ? 1 : _lessons.Max(v => v.Id) + 1;
                SetPrivateField(lesson, nameof(lesson.Id), id);
            }
            _lessons.Add(lesson);
        }

        public void Insert(List<Lesson> lessons)
        {
            int id = _lessons.Count == 0 ? 1 : _lessons.Max(l => l.Id) + 1;
            foreach (var lesson in lessons)
            {
                if (lesson.Id is default(int))
                {
                    SetPrivateField(lesson, nameof(lesson.Id), id);
                    id++;
                }
            }
            _lessons.AddRange(lessons);
        }

        public void Update(Lesson lesson)
        {
        }

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            typeof(T)
              .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(entity, value);
        }
    }
}
