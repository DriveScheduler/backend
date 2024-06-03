using Domain.Enums;
using Domain.Models.Vehicles;

namespace Domain.Repositories
{
    public interface IVehicleRepository
    {
        public int Insert(Vehicle vehicle);
        public List<int> Insert(List<Vehicle> vehicle);        
        public void Update(Vehicle vehicle);
        public Vehicle GetById(int id);        
        public Vehicle FindAvailable(DateTime start, int duration, LicenceType vehicleType);
        public bool IsRegistrationNumberUnique(string registrationNumber);
    }
}
