using Domain.Enums;
using Domain.Models.Vehicles;

namespace Infrastructure.Entities
{
    internal sealed class VehicleDataEntity : DataEntity<Vehicle>
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public LicenceType Type { get; set; }

        public List<LessonDataEntity> Lessons { get; set; }


        public VehicleDataEntity() { }
        public VehicleDataEntity(Vehicle domainModel) : base(domainModel) { }

        public override void FromDomainModel(Vehicle domainModel)
        {
            Id = domainModel.Id;
            RegistrationNumber = domainModel.RegistrationNumber.Value;
            Name = domainModel.Name;
            Type = domainModel.GetType();
            Lessons = domainModel.Lessons.Select(l => new LessonDataEntity(l)).ToList();
        }

        public override Vehicle ToDomainModel()
        {            
            if (Type == LicenceType.Car)            
                return new Car(Id, RegistrationNumber, Name);            
            else if (Type == LicenceType.Bus)            
                return new Bus(Id, RegistrationNumber, Name);            
            else if (Type == LicenceType.Truck)            
                return new Truck(Id, RegistrationNumber, Name);            
            else            
                return new Motorcycle(Id, RegistrationNumber, Name);          
        }
    }
}
