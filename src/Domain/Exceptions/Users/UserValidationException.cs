namespace Domain.Exceptions.Users
{
    public sealed class UserValidationException(string message) : Exception(message)
    {
    }
}
