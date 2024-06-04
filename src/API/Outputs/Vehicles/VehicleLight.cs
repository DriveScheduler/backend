using Domain.Models.Vehicles;

namespace API.Outputs.Vehicles
{
    public sealed class VehicleLight
    {
        public int Id { get; }
        public string RegistrationNumber { get; }
        public string Name { get; }
        public LicenceTypeOutput Type { get; }

        public VehicleLight(Vehicle vehicle)
        {
            Id = vehicle.Id;
            RegistrationNumber = vehicle.RegistrationNumber.Value;
            Name = vehicle.Name;
            Type = new LicenceTypeOutput(vehicle.GetType());
        }
    }
}
