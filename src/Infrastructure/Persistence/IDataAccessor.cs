using Domain.Models;

namespace Infrastructure.Persistence
{
    public interface IDataAccessor
    {
        public IQueryable<DrivingSchool> DrivingSchools { get; }
        public IQueryable<Lesson> Lessons { get; }
        public IQueryable<User> Users { get; }
        public IQueryable<Vehicle> Vehicles { get; }

        public void Insert<T>(T entity) where T : class;
        public void Insert<T>(List<T> entities) where T : class;
        public void Update<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;

        public void Clear();
    }
}
