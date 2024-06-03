namespace Domain.Enums
{
    public enum UserType
    {
        Student,
        Teacher,
        Admin
    }

    public static class UserTypeExtension
    {
        public static string ToString(this UserType userType) =>
             userType switch
             {
                 UserType.Student => "Elève",
                 UserType.Teacher => "Moniteur",
                 UserType.Admin => "Gestionnaire",
                 _ => string.Empty
             };
    }
}
