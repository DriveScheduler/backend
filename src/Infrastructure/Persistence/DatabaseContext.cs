using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal sealed class DatabaseContext : DbContext
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

        //public void Insert<T>(T entity) where T : class
        //{
        //    //Set<T>().Attach(entity);
        //    //ChangeTracker.Clear();
        //    Add(entity);
        //    //Set<T>().Add(entity);
        //    SaveChanges();
        //}
        //public void Insert<T>(List<T> entities) where T : class
        //{
        //    //Set<T>().AttachRange(entities);
        //    //ChangeTracker.Clear();
        //    //Set<T>().AddRange(entities);
        //    AddRange(entities);
        //    SaveChanges();
        //}

        //void Update<T>(T entity) where T : class
        //{
        //    //Set<T>().Attach(entity);
        //    //ChangeTracker.Clear();
        //    //Set<T>().Update(entity);
        //    //Update(entity);
        //    SaveChanges();
        //}

        //public void Delete<T>(T entity) where T : class
        //{
        //    //Set<T>().Attach(entity);
        //    //ChangeTracker.Clear();
        //    //Set<T>().Remove(entity);
        //    Remove(entity);
        //    SaveChanges();
        //}       


        internal DbSet<UserDataEntity> Users { get; set; }
        internal DbSet<LessonDataEntity> Lessons { get; set; }
        internal DbSet<VehicleDataEntity> Vehicles { get; set; }

        //IEnumerable<LessonDataEntity> IDataAccessor.Lessons => Lessons
        //    //.AsNoTracking()
        //    .Include(l => l.Teacher)
        //    .Include(l => l.Student)
        //    .Include(l => l.UserWaitingLists)
        //        .ThenInclude(u => u.User)
        //    .Include(l => l.Vehicle)
        //    .AsEnumerable();
        //    //.Select(l => l.FullDomainModel());

        //IEnumerable<UserDataEntity> IDataAccessor.Users => Users
        //    //.AsNoTracking()
        //    .Include(u => u.LessonsAsTeacher)
        //    .Include(u => u.LessonsAsStudent)
        //    .Include(u => u.LessonWaitingLists)
        //        .ThenInclude(l => l.Lesson)
        //    .AsEnumerable();
        //    //.Select(u => u.FullDomainModel());

        //IEnumerable<VehicleDataEntity> IDataAccessor.Vehicles => Vehicles
        //    //.AsNoTracking()
        //    .Include(v => v.Lessons)
        //    .AsEnumerable();
        //    //.Select(v => v.FullDomainModel());
    }
}
