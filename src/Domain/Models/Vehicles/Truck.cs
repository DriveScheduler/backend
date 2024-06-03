using Domain.Enums;

namespace Domain.Models.Vehicles
{
    public sealed class Truck : Vehicle
    {
        public Truck(string registrationNumber, string name) : base(registrationNumber, name)
        {
        }

        public Truck(int id, string registrationNumber, string name) : base(id, registrationNumber, name)
        {
        }

        public override LicenceType GetType() => LicenceType.Truck;        
    }
}
