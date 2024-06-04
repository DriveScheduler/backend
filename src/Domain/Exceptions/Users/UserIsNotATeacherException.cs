namespace Domain.Exceptions.Users
{
    public sealed class UserIsNotATeacherException : Exception
    {
        public UserIsNotATeacherException() : base("L'utilisateur n'est pas un moniteur") { }
    }
}
