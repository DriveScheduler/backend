using Domain.Entities.Database;

namespace API.Output.Users
{
    public sealed class UserLight
    {
        public Guid Id { get; }
        public string Name { get; }
        public string FirstName { get; }
        public string Email { get; }
        public Licence LicenceType { get; }

        public UserLight(User user)
        {
            Id = user.Id;
            Name = user.Name;
            FirstName = user.FirstName;
            Email = user.Email;
            LicenceType = new Licence(user.LicenceType);
        }
    }
}
