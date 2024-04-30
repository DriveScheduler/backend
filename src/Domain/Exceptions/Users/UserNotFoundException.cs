namespace Domain.Exceptions.Users
{
    public sealed class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("L'utilisateur n'existe pas")
        {
        }
    }
}
