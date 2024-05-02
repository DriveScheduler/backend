using Domain.Enums;

namespace Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public LicenceType LicenceType { get; set; }
    }
}
