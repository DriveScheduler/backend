using Domain.Enums;

namespace Domain.Entities
{
    public sealed class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LicenceType Type { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
