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
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        private DbSet<UserDataEntity> _users { get; set; }
        private DbSet<LessonDataEntity> _lessons { get; set; }
        private DbSet<VehicleDataEntity> _vehicles { get; set; }

        internal IQueryable<UserDataEntity> Users => _users
            .Include(u => u.LessonsAsTeacher)
            .Include(u => u.LessonsAsStudent)
            .Include(u => u.LessonWaitingLists)
                .ThenInclude(l => l.Lesson);

        internal IQueryable<LessonDataEntity> Lessons => _lessons
            .Include(l => l.Teacher)
            .Include(l => l.Student)
            .Include(l => l.UserWaitingLists)
                .ThenInclude(u => u.User)
            .Include(l => l.Vehicle);

        internal IQueryable<VehicleDataEntity> Vehicles => _vehicles
            .Include(v => v.Lessons);

    }
}
