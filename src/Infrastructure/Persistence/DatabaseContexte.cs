using Application.Abstractions;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal sealed class DatabaseContexte : DbContext, IDatabase
    {
        public DatabaseContexte(DbContextOptions<DatabaseContexte> options) : base(options)
        {
        }

        IQueryable<User> IDatabase.Users => Users;

        public async void AddAsync<T>(T entity) where T : class
        {
            await base.AddAsync(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            base.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContexte).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        internal DbSet<User> Users { get; set; }             
    }
}
