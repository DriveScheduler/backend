using Domain.Entities;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    // FULL LOAD
    internal sealed class DrivingSchoolRepository(DatabaseContext database) : IDrivingSchoolRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<DrivingSchool>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DrivingSchool> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(DrivingSchool drivingSchool)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DrivingSchool drivingSchool)
        {
            throw new NotImplementedException();
        }

        //public async Task<DrivingSchool> InsertAsync(DrivingSchool entity)
        //{
        //    DrivingSchool_Database dbDrivingSchool = new DrivingSchool_Database(entity);
        //    _database.DrivingSchools.Add(dbDrivingSchool);
        //    await _database.SaveChangesAsync();
        //    return DrivingSchool.Create(dbDrivingSchool.Id, entity.Name, entity.Address);
        //}

        //public Task UpdateAsync(DrivingSchool entity)
        //{
        //    DrivingSchool_Database? dbDrivingSchool = _database.DrivingSchools.Find(entity.Id);
        //    if (dbDrivingSchool is null)
        //        throw new DrivingSchoolNotFoundException();

        //    dbDrivingSchool.FromDomainModel(entity);

        //    return _database.SaveChangesAsync();
        //}

        //public Task DeleteAsync(int id)
        //{
        //    DrivingSchool_Database? drivingSchool = _database.DrivingSchools.Find(id);
        //    if (drivingSchool is not null)
        //    {
        //        _database.DrivingSchools.Remove(drivingSchool);
        //        return _database.SaveChangesAsync();
        //    }
        //    return Task.CompletedTask;
        //}

        //public async Task<DrivingSchool> GetByIdAsync(int id)
        //{
        //    DrivingSchool_Database? dbDrivingSchool = await _database.DrivingSchools.FindAsync(id);
        //    if (dbDrivingSchool is null)
        //        throw new DrivingSchoolNotFoundException();
        //    return dbDrivingSchool.ToDomainModel();
        //}        
    }
}
