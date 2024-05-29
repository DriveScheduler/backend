using Domain.Models;

namespace Domain.Repositories
{
    public interface IDrivingSchoolRepository
    {
        public void Insert(DrivingSchool drivingSchool);
        public void Update(DrivingSchool drivingSchool);
        public DrivingSchool GetById(int id);
        public List<DrivingSchool> GetAll();
    }
}
