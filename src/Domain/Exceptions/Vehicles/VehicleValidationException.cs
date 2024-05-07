namespace Domain.Exceptions.Vehicles
{
    public sealed class VehicleValidationException : Exception
    {
        public VehicleValidationException(string message) : base(message) { }
    }
}
