using Domain.Enums;

namespace Domain.Entities.Database
{
    public sealed class Vehicle
    {
        public int Id { get; set; }
        public required string RegistrationNumber { get; set; }
        public required string Name { get; set; }
        public required LicenceType Type { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
