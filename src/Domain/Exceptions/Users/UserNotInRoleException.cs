namespace Domain.Exceptions.Users
{
    public sealed class UserNotInRoleException : Exception
    {
        public UserNotInRoleException(string message) : base(message)
        {
        }
    }
}
