using Domain.Models;

namespace Domain.Repositories
{
    public interface IDrivingSchoolRepository
    {
        public Task<int> InsertAsync(DrivingSchool drivingSchool);
        public Task UpdateAsync(DrivingSchool drivingSchool);
        public Task<DrivingSchool> GetByIdAsync(int id);
        public Task<List<DrivingSchool>> GetAllAsync();
    }
}
