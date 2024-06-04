using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.Models.Vehicles;
using Domain.Repositories;

using System.Reflection;

namespace UseCases.Fakes.Repositories
{
    internal sealed class FakeVehicleRepository : IVehicleRepository
    {
        private List<Vehicle> _vehicles = [];

        public void Clear()
        {
            _vehicles = [];
        }

        public Vehicle FindAvailable(DateTime start, int duration, LicenceType vehicleType)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> GetAll()
        {
            return _vehicles;
        }

        public Vehicle GetById(int id)
        {
            Vehicle? vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle is null) throw new VehicleNotFoundException();
            return vehicle;
        }

        public List<Vehicle> GetVehiclesByType(LicenceType vehicleType)
        {
            return _vehicles.Where(v => v.GetType() == vehicleType).ToList();
        }

        public void Insert(Vehicle vehicle)
        {
            if (vehicle.Id is default(int))
            {
                int id = _vehicles.Count == 0 ? 1 : _vehicles.Max(v => v.Id) + 1;
                SetPrivateField(vehicle, nameof(vehicle.Id), id);
            }
            _vehicles.Add(vehicle);

        }

        public void Insert(List<Vehicle> vehicles)
        {
            int id = _vehicles.Count == 0 ? 1 : _vehicles.Max(l => l.Id) + 1;
            foreach (var vehicle in vehicles)
            {
                if (vehicle.Id is default(int))
                {
                    SetPrivateField(vehicle, nameof(vehicle.Id), id);
                    id++;
                }
            }
            _vehicles.AddRange(vehicles);
        }

        public bool IsRegistrationNumberUnique(string registrationNumber)
        {
            return _vehicles.FirstOrDefault(v => v.RegistrationNumber.Value == registrationNumber) is null;
        }

        public void Update(Vehicle vehicle)
        {

        }

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            typeof(T)
              .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(entity, value);
        }
    }
}
