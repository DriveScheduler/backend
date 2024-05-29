namespace Domain.Exceptions.Users
{
    public sealed class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email) : base($"The email {email} is invalid.") { }        
    }
}
