using Domain.Exceptions.DrivingSchools;
using Domain.Exceptions.Users;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    internal sealed class DrivingSchoolRepository(IDataAccessor database) : IDrivingSchoolRepository
    {
        private readonly IDataAccessor _database = database;

        public List<DrivingSchool> GetAll()
        {
            return _database.DrivingSchools
                .ToList();
        }

        public DrivingSchool GetById(int id)
        {
            DrivingSchool? drivingSchool = _database.DrivingSchools.FirstOrDefault(d => d.Id == id);
            if (drivingSchool is null)
                throw new DrivingSchoolNotFoundException();
            return drivingSchool;
        }

        public void Insert(DrivingSchool drivingSchool)
        {
            try
            {
                _database.Insert(drivingSchool);
            }
            catch (Exception)
            {
                throw new DrivingSchoolSaveException();
            }
        }

        public void Update(DrivingSchool drivingSchool)
        {
            try
            {
                _database.Update(drivingSchool);
            }
            catch (Exception)
            {
                throw new UserSaveException();
            }
        }
    }
}
