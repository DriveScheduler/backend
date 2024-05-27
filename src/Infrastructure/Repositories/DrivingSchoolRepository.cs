using Domain.Exceptions.DrivingSchools;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal sealed class DrivingSchoolRepository(DatabaseContext database) : IDrivingSchoolRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<DrivingSchool>> GetAllAsync()
        {
            return _database.DrivingSchools
                .Select(d => d.ToDomainModel())
                .ToListAsync();
        }

        public async Task<DrivingSchool> GetByIdAsync(int id)
        {
            DrivingSchool_Database? dbDrivingSchool = await _database.DrivingSchools.FindAsync(id);
            if (dbDrivingSchool is null)
                throw new DrivingSchoolNotFoundException();
            return dbDrivingSchool.ToDomainModel();
        }

        public async Task<int> InsertAsync(DrivingSchool drivingSchool)
        {
            DrivingSchool_Database dbDrivingSchool = new(drivingSchool);           
            try
            {
                _database.DrivingSchools.Add(dbDrivingSchool);
                await _database.SaveChangesAsync();
                return dbDrivingSchool.Id;
            }
            catch (Exception)
            {
                throw new DrivingSchoolSaveException();
            }
        }

        public async Task UpdateAsync(DrivingSchool drivingSchool)
        {
            DrivingSchool_Database? dbDrivingSchool = await _database.DrivingSchools.FindAsync(drivingSchool.Id);
            if (dbDrivingSchool is null)
                throw new DrivingSchoolNotFoundException();

            dbDrivingSchool.FromDomainModel(drivingSchool);
            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DrivingSchoolSaveException();
            }
        }
    }
}
