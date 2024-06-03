using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal sealed class DatabaseContext : DbContext, IDataAccessor
    {
        public DatabaseContext()
        {            
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {            
            //ChangeTracker.AutoDetectChangesEnabled = false;
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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

        public void Insert<T>(T entity) where T : class
        {
            //Set<T>().Attach(entity);
            Set<T>().Add(entity);
            SaveChanges();
        }
        public void Insert<T>(List<T> entities) where T : class
        {
            //Set<T>().AttachRange(entities);
            Set<T>().AddRange(entities);
            SaveChanges();
        }

        void IDataAccessor.Update<T>(T entity) where T : class
        {
            //Set<T>().Attach(entity);
            Set<T>().Update(entity);
            SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            //Set<T>().Attach(entity);
            Set<T>().Remove(entity);
            SaveChanges();
        }


        internal DbSet<UserDataEntity> Users { get; set; }
        internal DbSet<LessonDataEntity> Lessons { get; set; }
        internal DbSet<VehicleDataEntity> Vehicles { get; set; }

        IEnumerable<Lesson> IDataAccessor.Lessons => Lessons
            .Include(l => l.Teacher)
            .Include(l => l.Student)
            .Include(l => l.WaitingList)
            .Include(l => l.Vehicle)
            //.AsNoTracking()
            .AsEnumerable()
            .Select(l => l.ToDomainModel());

        IEnumerable<User> IDataAccessor.Users => Users
            .Include(u => u.LessonsAsTeacher)
            .Include(u => u.LessonsAsStudent)
            .Include(u => u.WaitingList)
            //.AsNoTracking()
            .AsEnumerable()
            .Select(u => u.ToDomainModel());

        IEnumerable<Vehicle> IDataAccessor.Vehicles => Vehicles
            .Include(v => v.Lessons)
            //.AsNoTracking()
            .AsEnumerable()
            .Select(v => v.ToDomainModel());
    }
}
