using Domain.Enums;

namespace Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public LicenceType LicenceType { get; set; }
        public UserType UserType { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
