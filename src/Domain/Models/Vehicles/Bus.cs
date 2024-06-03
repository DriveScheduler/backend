using Domain.Enums;

namespace Domain.Models.Vehicles
{
    public sealed class Bus : Vehicle
    {
        public Bus(string registrationNumber, string name) : base(registrationNumber, name)
        {
        }

        public Bus(int id, string registrationNumber, string name) : base(id, registrationNumber, name)
        {
        }

        public override LicenceType GetType() => LicenceType.Bus;
    }
}
