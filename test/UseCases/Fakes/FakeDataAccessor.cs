//using Domain.Models;
//using Domain.Models.Users;
//using Domain.Models.Vehicles;
//using Infrastructure.Persistence;

//using System.Reflection;

//namespace UseCases.Fakes
//{
//    public sealed class FakeDataAccessor : IDataAccessor
//    {        
//        public IEnumerable<Lesson> Lessons => _lessons;

//        public IEnumerable<User> Users => _users;

//        public IEnumerable<Vehicle> Vehicles => _vehicles;

//        public void Delete<T>(T entity) where T : class
//        {
//            throw new NotImplementedException();
//        }

//        public void Insert<T>(T entity) where T : class
//        {           
//            if (entity is Lesson lesson)
//            {
//                if(lesson.Id is default(int))
//                {
//                    int id = _lessons.Count == 0 ? 1 : _lessons.Max(l => l.Id) + 1;
//                    SetPrivateField(lesson, nameof(lesson.Id), id);
//                }                
//                List<Lesson> teacherLesson = lesson.Teacher.Lessons.ToList();
//                teacherLesson.Add(lesson);
//                SetPrivateField(lesson.Teacher, "LessonsAsTeacher", teacherLesson.AsReadOnly());

//                if(lesson.Student != null)
//                {
//                    List<Lesson> studentLesson = lesson.Student.Lessons.ToList();
//                    studentLesson.Add(lesson);
//                    SetPrivateField(lesson.Student, "LessonsAsStudent", studentLesson.AsReadOnly());
//                }

//                _lessons.Add(lesson);
//            }
//            else if (entity is User user)
//            {
//                if (user.Id == Guid.Empty)
//                {
//                    Guid id = Guid.NewGuid();
//                    SetPrivateField(user, nameof(user.Id), id);

//                }
//                _users.Add(user);
//            }
//            else if (entity is Vehicle vehicle)
//            {
//                if(vehicle.Id is default(int))
//                {
//                    int id = _vehicles.Count == 0 ? 1 : _vehicles.Max(v => v.Id) + 1;
//                    SetPrivateField(vehicle, nameof(vehicle.Id), id);                    
//                }
//                _vehicles.Add(vehicle);
//            }
//            else
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public void Insert<T>(List<T> entities) where T : class
//        {
//            if(entities is List<Lesson> lessons)
//            {
//                //int id = _lessons.Count == 0 ? 1 : _lessons.Max(l => l.Id) + 1;
//                //foreach (var lesson in lessons)
//                //{

//                //    SetPrivateField(lesson, nameof(lesson.Id), id);
//                //    id++;
//                //}
//                _lessons.AddRange(lessons);
//            }
//            else if(entities is List<User> users)
//            {
//                //foreach (var user in users)
//                //{
//                //    SetPrivateField(user, nameof(user.Id), Guid.NewGuid());
//                //}
//                _users.AddRange(users);
//            }
//            else if(entities is List<Vehicle> vehicles)
//            {
//                //int id = _vehicles.Count == 0 ? 1 : _vehicles.Max(l => l.Id) + 1;
//                //foreach (var vehicle in vehicles)
//                //{
//                //    SetPrivateField(vehicle, nameof(vehicle.Id), id);
//                //    id++;
//                //}
//                _vehicles.AddRange(vehicles);
//            }
//            else
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public void Update<T>(T entity) where T : class { }        

//        public void Clear()
//        {            
//            _lessons = [];
//            _users = [];
//            _vehicles = [];
//        }

//        private void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
//        {
//            var field = typeof(T).GetField($"<{fieldName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
//            field?.SetValue(entity, value);
//        }
        
//        private List<Lesson> _lessons = [];
//        private List<User> _users = [];
//        private List<Vehicle> _vehicles = [];
//    }
//}
