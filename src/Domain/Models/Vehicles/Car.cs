using Domain.Enums;

namespace Domain.Models.Vehicles
{
    public sealed class Car : Vehicle
    {
        public Car(string registrationNumber, string name) : base(registrationNumber, name)
        {
        }

        public Car(int id, string registrationNumber, string name) : base(id, registrationNumber, name)
        {
        }

        public override LicenceType GetType() => LicenceType.Car;        
    }
}
