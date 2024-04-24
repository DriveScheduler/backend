namespace Domain.Exceptions.Users
{
    public sealed class UserSaveException : Exception
    {
        public UserSaveException() : base("Une erreur s'est produite pendant la sauvegarde de l'utilisateur")
        {
        }
    }
}
