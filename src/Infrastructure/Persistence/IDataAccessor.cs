using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;

using Infrastructure.Entities;

namespace Infrastructure.Persistence
{
    public interface IDataAccessor
    {
        public IEnumerable<Lesson> Lessons { get; }
        public IEnumerable<User> Users { get; }
        public IEnumerable<Vehicle> Vehicles { get; }

        public void Insert<T>(T entity) where T : class;
        public void Insert<T>(List<T> entities) where T : class;
        public void Update<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;

        public void Clear();
    }
}
