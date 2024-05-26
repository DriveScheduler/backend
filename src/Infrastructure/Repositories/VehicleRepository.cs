using Domain.Entities;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    // FULL LOAD
    internal sealed class VehicleRepository(DatabaseContext database) : IVehicleRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<Vehicle> GetVehicleByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        //public Task<int> Delete(int id)
        //{
        //    _database.Vehicles.Remove(_database.Vehicles.Find(id));
        //    return _database.SaveChangesAsync();
        //}

        //public Task<int> InsertAsync(Vehicle entity)
        //{            
        //    _database.Vehicles.Add(new Vehicle_Database(entity));
        //    return _database.SaveChangesAsync();
        //}

        //public Task<int> Update(Vehicle entity)
        //{
        //    Vehicle_Database? dbVehicle = _database.Vehicles.Find(entity.Id);
        //    if(dbVehicle is null) return Task.FromResult(1);
        //    dbVehicle.FromDomainModel(entity);
        //    return _database.SaveChangesAsync();
        //}
    }
}
