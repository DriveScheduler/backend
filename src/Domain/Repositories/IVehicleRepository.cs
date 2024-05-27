﻿using Domain.Models;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IVehicleRepository
    {
        public int Insert(Vehicle vehicle);
        public List<int> Insert(List<Vehicle> vehicle);
        public Task<int> InsertAsync(Vehicle vehicle);
        public Task UpdateAsync(Vehicle vehicle);
        public Task<Vehicle> GetByIdAsync(int id);        
        public Task<Vehicle> FindAvailable(DateTime start, int duration, LicenceType vehicleType);
    }
}
