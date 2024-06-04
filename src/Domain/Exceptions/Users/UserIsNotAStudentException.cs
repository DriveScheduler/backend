namespace Domain.Exceptions.Users
{
    public sealed class UserIsNotAStudentException : Exception
    {
        public UserIsNotAStudentException() : base("L'utilisateur n'est pas un élève")
        {
        }
    }
}
