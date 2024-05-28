using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal sealed class DatabaseContext : DbContext, IDataAccessor
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\Romain\\Documents\\UnitTests\\DriveScheduler\\backend\\src\\Infrastructure\\database.db");
            base.OnConfiguring(optionsBuilder);
        }

        public void Insert<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
            SaveChanges();
        }
        public void Insert<T>(List<T> entities) where T : class
        {
            Set<T>().AddRange(entities);
            SaveChanges();
        }

        void IDataAccessor.Update<T>(T entity) where T : class
        {
            Set<T>().Update(entity);
            SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
            SaveChanges();
        }

      

        internal DbSet<User> Users { get; set; }        
        internal DbSet<Lesson> Lessons { get; set; }
        internal DbSet<Vehicle> Vehicles { get; set; }
        internal DbSet<DrivingSchool> DrivingSchools { get; set; }

        IQueryable<DrivingSchool> IDataAccessor.DrivingSchools => DrivingSchools;            

        IQueryable<Lesson> IDataAccessor.Lessons => Lessons
            .Include(l => l.Teacher)
            .Include(l => l.Student)
            .Include(l => l.WaitingList)
            .Include(l => l.Vehicle);            

        IQueryable<User> IDataAccessor.Users => Users
            .Include(u => u.LessonsAsTeacher)
            .Include(u => u.LessonsAsStudent)
            .Include(u => u.WaitingList);            

        IQueryable<Vehicle> IDataAccessor.Vehicles => Vehicles
            .Include(v => v.Lessons);       
    }
}
