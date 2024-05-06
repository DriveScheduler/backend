using Domain.Enums;

namespace Domain.Entities
{
    public sealed class Vehicle
    {
        public required string RegistrationNumber { get; set; }
        public required string Name { get; set; }
        public required LicenceType Type { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
