using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Domain.Abstractions
{
    public interface IDatabase
    {
        DbSet<Student> Students { get; }
        DbSet<Teacher> Teachers { get; }
        DbSet<Lesson> Lessons { get; }
        DbSet<Vehicle> Vehicles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Clear();
    }
}
