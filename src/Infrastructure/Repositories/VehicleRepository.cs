using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Vehicles;
using Domain.Models.Vehicles;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public sealed class VehicleRepository(IDataAccessor database) : IVehicleRepository
    {
        private readonly IDataAccessor _database = database;

        public Vehicle FindAvailable(DateTime start, int duration, LicenceType vehicleType)
        {
            List<Vehicle> vehicles = _database.Vehicles.Where(v => v.GetType() == vehicleType).ToList();

            DateTime lessonEnd = start.AddMinutes(duration);
            Vehicle? vehicle = vehicles.FirstOrDefault(v => !v.Lessons.Any(lesson => (lesson.Start.AddMinutes(lesson.Duration.Value) >= start && start >= lesson.Start) || (lesson.Start <= lessonEnd && lessonEnd <= lesson.Start.AddMinutes(lesson.Duration.Value))));
            if (vehicle is null)
                throw new LessonValidationException("Aucun vehicule disponibe pour valider ce cours");

            return vehicle;
        }

        public Vehicle GetById(int id)
        {
            Vehicle? vehicle = _database.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle is null) throw new VehicleNotFoundException();
            return vehicle;
        }

        public bool IsRegistrationNumberUnique(string registrationNumber)
        {
            return _database.Vehicles.FirstOrDefault(v => v.RegistrationNumber.Value == registrationNumber) is null;
        }


        public int Insert(Vehicle vehicle)
        {
            try
            {
                VehicleDataEntity vehicleDataEntity = new VehicleDataEntity(vehicle);
                _database.Insert(vehicleDataEntity);
                return vehicleDataEntity.Id;
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        public List<int> Insert(List<Vehicle> vehicle)
        {
            try
            {
                List<VehicleDataEntity> vehicleDataEntities = vehicle.Select(v => new VehicleDataEntity(v)).ToList();
                _database.Insert(vehicleDataEntities);
                return vehicleDataEntities.Select(v => v.Id).ToList();
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }
      
        public void Update(Vehicle vehicle)
        {
            try
            {
                _database.Update(new VehicleDataEntity(vehicle));
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }
    }
}
