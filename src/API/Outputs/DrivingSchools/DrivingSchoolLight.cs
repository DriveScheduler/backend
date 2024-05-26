using Domain.Entities;

namespace API.Outputs.DrivingSchools
{
    public sealed class DrivingSchoolLight
    {
          public int Id { get; }
          public string Name { get; }
          public string Address { get; }
    
          public DrivingSchoolLight(DrivingSchool drivingSchool)
          {
                Id = drivingSchool.Id;
                Name = drivingSchool.Name;
                Address = drivingSchool.Address;
          }
    }
}