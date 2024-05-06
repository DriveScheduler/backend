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

        DbSet<User> IDatabase.Users => Users;        
        DbSet<Lesson> IDatabase.Lessons => Lessons;
        DbSet<Vehicle> IDatabase.Vehicles => Vehicles;

        public async new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void Clear()
        {
            Users.RemoveRange(Users.ToList());            
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

        internal DbSet<User> Users { get; set; }        
        internal DbSet<Lesson> Lessons { get; set; }
        internal DbSet<Vehicle> Vehicles { get; set; }
    }
}
