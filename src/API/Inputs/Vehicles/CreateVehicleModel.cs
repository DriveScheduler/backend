using Domain.Enums;

namespace API.Inputs.Vehicles
{
    public sealed class CreateVehicleModel
    {
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public LicenceType Type { get; set; }
    }
}
