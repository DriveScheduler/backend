using Domain.Models;

using Infrastructure.Persistence;

namespace UseCases.Fakes
{
    internal sealed class FakeDataAccessor : IDataAccessor
    {
        public IEnumerable<DrivingSchool> DrivingSchools => _drivingSchools;

        public IEnumerable<Lesson> Lessons => _lessons;

        public IEnumerable<User> Users => _users;

        public IEnumerable<Vehicle> Vehicles => _vehicles;

        public void Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(T entity) where T : class
        {
            if (entity is DrivingSchool drivingSchool)
            {
                _drivingSchools.Add(drivingSchool);
            }
            else if (entity is Lesson lesson)
            {
                _lessons.Add(lesson);
            }
            else if (entity is User user)
            {
                _users.Add(user);
            }
            else if (entity is Vehicle vehicle)
            {
                _vehicles.Add(vehicle);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Insert<T>(List<T> entities) where T : class
        {
            if(entities is List<DrivingSchool> drivingSchools)
            {
                _drivingSchools.AddRange(drivingSchools);
            }
            else if(entities is List<Lesson> lessons)
            {
                _lessons.AddRange(lessons);
            }
            else if(entities is List<User> users)
            {
                _users.AddRange(users);
            }
            else if(entities is List<Vehicle> vehicles)
            {
                _vehicles.AddRange(vehicles);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Update<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _drivingSchools.Clear();
            _lessons.Clear();
            _users.Clear();
            _vehicles.Clear();
        }

        private readonly List<DrivingSchool> _drivingSchools = [];
        private readonly List<Lesson> _lessons = [];
        private readonly List<User> _users = [];
        private readonly List<Vehicle> _vehicles = [];
    }
}
