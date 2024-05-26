using Domain.Entities;

namespace API.Outputs.Users
{
    public sealed class UserLight
    {
        public Guid Id { get; }
        public string Name { get; }
        public string FirstName { get; }
        public string Email { get; }
        public LicenceTypeOutput LicenceType { get; }
        public UserTypeOutput UserType { get; }

        public UserLight(User user)
        {
            Id = user.Id;
            Name = user.Name;
            FirstName = user.FirstName;
            Email = user.Email;
            LicenceType = new LicenceTypeOutput(user.LicenceType);
            UserType = new UserTypeOutput(user.Type);
        }
    }
}
