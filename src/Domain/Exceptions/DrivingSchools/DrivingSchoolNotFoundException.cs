namespace Domain.Exceptions.DrivingSchools;

public class DrivingSchoolNotFoundException : Exception
{
    public DrivingSchoolNotFoundException() : base("L'auto école n'existe pas")
    {
    }
}