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
        public List<int> LessonsId { get; set; } = [];
        public List<LessonDataEntity> Lessons { get; set; }


        private VehicleDataEntity() { }
        public VehicleDataEntity(Vehicle domainModel) : base(domainModel) { }

        public override void FromDomainModel(Vehicle domainModel)
        {
            Id = domainModel.Id;
            RegistrationNumber = domainModel.RegistrationNumber.Value;
            Name = domainModel.Name;
            Type = domainModel.GetType();
            LessonsId = domainModel.Lessons.Select(l => l.Id).ToList();            
        }

        public override Vehicle ToDomainModel(int level)
        {
            if (level >= 2) return null;
            level++;

            Vehicle vehicle;
            if (Type == LicenceType.Car)
                vehicle = new Car(Id, RegistrationNumber, Name);
            else if (Type == LicenceType.Bus)
                vehicle = new Bus(Id, RegistrationNumber, Name);
            else if (Type == LicenceType.Truck)
                vehicle = new Truck(Id, RegistrationNumber, Name);
            else
                vehicle = new Motorcycle(Id, RegistrationNumber, Name);
            SetPrivateField(vehicle, "_lessons", Lessons.Select(l => l.ToDomainModel(level)).ToList());
            return vehicle;
        }
    }
}
