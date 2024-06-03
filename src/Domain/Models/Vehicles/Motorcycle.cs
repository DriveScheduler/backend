using Domain.Enums;

namespace Domain.Models.Vehicles
{
    public sealed class Motorcycle : Vehicle
    {
        public Motorcycle(string registrationNumber, string name) : base(registrationNumber, name)
        {
        }

        public Motorcycle(int id, string registrationNumber, string name) : base(id, registrationNumber, name)
        {
        }

        public override LicenceType GetType() => LicenceType.Motorcycle;        
    }
}
