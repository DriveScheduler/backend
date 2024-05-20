namespace Domain.Exceptions.DrivingSchools;

public class DrivingSchoolValidationException : Exception
{
    public DrivingSchoolValidationException(string message) : base(message) { }
}