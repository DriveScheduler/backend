namespace Domain.Exceptions.Vehicles
{
    public sealed class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException() : base("Le vehicule n'existe pas")
        {
        }
    }
}
