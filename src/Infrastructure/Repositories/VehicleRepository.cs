using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Vehicles;
using Domain.Models.Vehicles;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using System.Reflection;

namespace Infrastructure.Repositories
{
    internal sealed class VehicleRepository(DatabaseContext database) : IVehicleRepository
    {
        private readonly DatabaseContext _database = database;
       

        //public Vehicle GetById(int id)
        //{
        //    VehicleDataEntity? vehicle = _database.Vehicles.Find(id);
        //    if (vehicle is null) throw new VehicleNotFoundException();
        //    return vehicle.FullDomainModel();
        //}

        //public bool IsRegistrationNumberUnique(string registrationNumber)
        //{
        //    return _database.Vehicles.FirstOrDefault(v => v.RegistrationNumber == registrationNumber) is null;
        //}


        public void Insert(Vehicle vehicle)
        {
            try
            {
                VehicleDataEntity vehicleDataEntity = new VehicleDataEntity(vehicle);
                _database.Vehicles.Add(vehicleDataEntity);
                _database.SaveChanges();
                SetPrivateField(vehicle, nameof(Vehicle.Id), vehicleDataEntity.Id);                
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        public void Insert(List<Vehicle> vehicle)
        {
            try
            {
                List<VehicleDataEntity> vehicleDataEntities = vehicle.Select(v => new VehicleDataEntity(v)).ToList();
                _database.Vehicles.AddRange(vehicleDataEntities);
                _database.SaveChanges();
                for (int i = 0; i < vehicleDataEntities.Count; i++)
                    SetPrivateField(vehicle[i], nameof(Vehicle.Id), vehicleDataEntities[i].Id);                
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
                VehicleDataEntity? dataEntity = _database.Vehicles.Find(vehicle.Id);
                if (dataEntity is null) 
                    throw new VehicleNotFoundException();
                dataEntity.FromDomainModel(vehicle);
                _database.SaveChanges();
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        private static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            var field = typeof(T).GetField($"<{fieldName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(entity, value);
        }

        public Vehicle GetById(int id)
        {
            VehicleDataEntity? dataEntity = _database.Vehicles.Find(id);
            if (dataEntity is null)
                throw new VehicleNotFoundException();
            return dataEntity.FullDomainModel();
        }

        public List<Vehicle> GetAll()
        {
            return _database.Vehicles
                .Select(v => v.FullDomainModel())
                .ToList();
        }

        public List<Vehicle> GetVehiclesByType(LicenceType vehicleType)
        {
            return _database.Vehicles
                .Where(v => v.Type == vehicleType)
                .Select(v => v.FullDomainModel())
                .ToList();
        }

        public Vehicle FindAvailable(DateTime start, int duration, LicenceType vehicleType)
        {
            List<Vehicle> vehicles = GetVehiclesByType(vehicleType);

            DateTime lessonEnd = start.AddMinutes(duration);
            Vehicle? vehicle = vehicles.FirstOrDefault(v => !v.Lessons.Any(lesson => (lesson.Start.AddMinutes(lesson.Duration.Value) >= start && start >= lesson.Start) || (lesson.Start <= lessonEnd && lessonEnd <= lesson.Start.AddMinutes(lesson.Duration.Value))));
            if (vehicle is null)
                throw new LessonValidationException("Aucun vehicule disponibe pour valider ce cours");

            return vehicle;
        }

        public bool IsRegistrationNumberUnique(string registrationNumber)
        {
            return _database.Vehicles.FirstOrDefault(v => v.RegistrationNumber == registrationNumber) is null;
        }
    }
}
