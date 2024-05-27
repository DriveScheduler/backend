using Domain.Models;
using Domain.Enums;
using Domain.Repositories;

using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;
using Domain.Exceptions.Lessons;
using Domain.Exceptions.Vehicles;

namespace Infrastructure.Repositories
{    
    internal sealed class VehicleRepository(DatabaseContext database) : IVehicleRepository
    {
        private readonly DatabaseContext _database = database;

        public async Task<Vehicle> FindAvailable(DateTime start, int duration, LicenceType vehicleType)
        {
            List<Vehicle_Database> vehicles = await IncludeAllSubEntities().Where(v => v.Type == vehicleType).ToListAsync();                

            DateTime lessonEnd = start.AddMinutes(duration);
            Vehicle_Database? vehicle = vehicles.FirstOrDefault(v => !v.Lessons.Any(lesson => (lesson.Start.AddMinutes(lesson.Duration) >= start && start >= lesson.Start) || (lesson.Start <= lessonEnd && lessonEnd <= lesson.Start.AddMinutes(lesson.Duration))));
            if (vehicle is null)
                throw new LessonValidationException("Aucun vehicule disponibe pour valider ce cours");

            return vehicle.ToDomainModel();
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            Vehicle_Database? vehicle = await IncludeAllSubEntities().FirstOrDefaultAsync(v => v.Id == id);
            if(vehicle is null) throw new VehicleNotFoundException();
            return vehicle.ToDomainModel();
        }       

        public int Insert(Vehicle vehicle)
        {
            Vehicle_Database dbVehicle = new(vehicle);
            try
            {
                _database.Vehicles.Add(dbVehicle);
                _database.SaveChanges();
                return dbVehicle.Id;
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        public List<int> Insert(List<Vehicle> vehicle)
        {
            List<Vehicle_Database> dbVehicles = vehicle.Select(v => new Vehicle_Database(v)).ToList();
            try
            {
                _database.Vehicles.AddRange(dbVehicles);
                _database.SaveChangesAsync();
                return dbVehicles.Select(v => v.Id).ToList();
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        public async Task<int> InsertAsync(Vehicle vehicle)
        {
            Vehicle_Database dbVehicle = new(vehicle);
            try
            {
                _database.Vehicles.Add(dbVehicle);
                await _database.SaveChangesAsync();
                return dbVehicle.Id;
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            Vehicle_Database? dbVehicle = _database.Vehicles.Find(vehicle.Id);
            if(dbVehicle is null) throw new VehicleNotFoundException();
            dbVehicle.FromDomainModel(vehicle);
            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new VehicleSaveException();
            }
        }

        private IQueryable<Vehicle_Database> IncludeAllSubEntities()
        {
            return _database.Vehicles
                .Include(v => v.Lessons);                
        }      
    }
}
