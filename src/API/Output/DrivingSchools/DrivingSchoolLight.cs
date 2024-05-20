using Domain.Entities.Database;

namespace API.Output.DrivingSchools
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