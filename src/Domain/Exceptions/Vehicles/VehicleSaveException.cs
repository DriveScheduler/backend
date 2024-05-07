namespace Domain.Exceptions.Vehicles
{
    public sealed class VehicleSaveException : Exception
    {
        public VehicleSaveException() : base("Une erreur s'est produite pendant la sauvegarde du véhicule") { }
    }
}
