using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Domain.Abstractions
{
    public interface IDatabase
    {
        DbSet<User> Users { get; }
        DbSet<Lesson> Lessons { get; }
        DbSet<Vehicle> Vehicles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Clear();
    }
}
