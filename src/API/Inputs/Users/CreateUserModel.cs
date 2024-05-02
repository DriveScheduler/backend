using Domain.Enums;

namespace API.Inputs.Users
{
    public sealed class CreateUserModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public LicenceType LicenceType { get; set; }        
    }
}
