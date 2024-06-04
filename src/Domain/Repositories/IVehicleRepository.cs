using Domain.Models;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IVehicleRepository
    {
        public void Insert(Vehicle vehicle);
        public void Insert(List<Vehicle> vehicle);        
        public void Update(Vehicle vehicle);
        public Vehicle GetById(int id);        
        public List<Vehicle> GetAll();
        public Vehicle FindAvailable(DateTime start, int duration, LicenceType vehicleType);
        public bool IsRegistrationNumberUnique(string registrationNumber);
    }
}
