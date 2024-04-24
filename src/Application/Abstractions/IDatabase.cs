using Domain.Entities;

namespace Application.Abstractions
{
    public interface IDatabase
    {
        void AddAsync<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<int> SaveChangesAsync();

        IQueryable<User> Users { get; }
    }
}
