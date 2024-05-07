using Domain.Enums;

namespace API.Inputs.Vehicles
{
    public class UpdateVehicleModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public LicenceType Type { get; set; }
    }
}
