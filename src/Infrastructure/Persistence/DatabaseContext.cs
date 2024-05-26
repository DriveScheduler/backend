using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal sealed class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }        

        public async new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void Clear()
        {
            Users.RemoveRange(Users.ToList());            
            Lessons.RemoveRange(Lessons.ToList());
            Vehicles.RemoveRange(Vehicles.ToList());
            DrivingSchools.RemoveRange(DrivingSchools.ToList());
            SaveChanges();

            ChangeTracker.Clear();
            Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
        internal DbSet<User_Database> Users { get; set; }        
        internal DbSet<Lesson_Database> Lessons { get; set; }
        internal DbSet<Vehicle_Database> Vehicles { get; set; }
        internal DbSet<DrivingSchool_Database> DrivingSchools { get; set; }
    }
}
