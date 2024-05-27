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

        //public async new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        //{
        //    return await base.SaveChangesAsync(cancellationToken);
        //}

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

        //public void Insert<T>(T entity) where T : class
        //{
        //    Set<T>().Add(entity);
        //}

        //void IDataAccess.Update<T>(T entity)
        //{
        //    Set<T>().Update(entity);
        //}

        //public void Delete<T>(T entity) where T : class
        //{
            //Set<T>().Remove(entity);
        //}

        internal DbSet<User_Database> Users { get; set; }        
        internal DbSet<Lesson_Database> Lessons { get; set; }
        internal DbSet<Vehicle_Database> Vehicles { get; set; }
        internal DbSet<DrivingSchool_Database> DrivingSchools { get; set; }

        //IQueryable<DrivingSchool> IDataAccess.DrivingSchools => DrivingSchools.AsQueryable().Select(d => d.ToDomainModel());

        //IQueryable<Lesson> IDataAccess.Lessons => Lessons.AsQueryable().Select(l => l.ToDomainModel());

        //IQueryable<User> IDataAccess.Users => Users.AsQueryable().Select(u => u.ToDomainModel());

        //IQueryable<Vehicle> IDataAccess.Vehicles => Vehicles.AsQueryable().Select(v => v.ToDomainModel());
    }
}
