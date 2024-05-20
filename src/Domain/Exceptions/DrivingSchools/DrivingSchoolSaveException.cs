namespace Domain.Exceptions.DrivingSchools
{
    public sealed class DrivingSchoolSaveException : Exception
    {
        public DrivingSchoolSaveException() : base("Une erreur s'est produite pendant la sauvegarde de l'auto école") { }
    }
}