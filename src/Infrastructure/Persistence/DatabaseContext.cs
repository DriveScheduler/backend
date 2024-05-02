using Domain.Abstractions;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal sealed class DatabaseContext : DbContext, IDatabase
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        DbSet<Student> IDatabase.Students => Students;
        DbSet<Teacher> IDatabase.Teachers => Teachers;
        DbSet<Lesson> IDatabase.Lessons => Lessons;
        DbSet<Vehicle> IDatabase.Vehicles => Vehicles;

        public async new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void Clear()
        {
            Students.RemoveRange(Students.ToList());
            Teachers.RemoveRange(Teachers.ToList());
            Lessons.RemoveRange(Lessons.ToList());
            Vehicles.RemoveRange(Vehicles.ToList());
            SaveChanges();

            ChangeTracker.Clear();
            Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }        

        internal DbSet<Student> Students { get; set; }
        internal DbSet<Teacher> Teachers { get; set; }
        internal DbSet<Lesson> Lessons { get; set; }
        internal DbSet<Vehicle> Vehicles { get; set; }
    }
}
