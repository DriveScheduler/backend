using Domain.Entities;

namespace Domain.Repositories
{
    public interface IVehicleRepository
    {
        public Task<int> InsertAsync(Vehicle vehicle);
        public Task UpdateAsync(Vehicle vehicle);
        public Task<Vehicle> GetVehicleByIdAsync(int id);
        public Task<Vehicle> FindAvailable(DateTime start, int duration);
    }
}
