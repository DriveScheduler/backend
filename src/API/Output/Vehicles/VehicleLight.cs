using Domain.Entities.Database;

namespace API.Output.Vehicles
{
    public sealed class VehicleLight
    {
        public int Id { get; }
        public string RegistrationNumber { get; }
        public string Name { get; }
        public Licence Type { get; }

        public VehicleLight(Vehicle vehicle)
        {
            Id = vehicle.Id;
            RegistrationNumber = vehicle.RegistrationNumber;
            Name = vehicle.Name;
            Type = new Licence(vehicle.Type);
        }
    }
}
