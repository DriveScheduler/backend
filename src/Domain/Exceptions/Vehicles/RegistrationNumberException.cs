namespace Domain.Exceptions.Vehicles
{
    public sealed class RegistrationNumberException : Exception
    {
        public RegistrationNumberException() : base("L'immatriculation ne respecte pas le format XX123XX") { }
    }
}
