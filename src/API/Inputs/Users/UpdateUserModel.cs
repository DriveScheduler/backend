using Domain.Enums;

namespace API.Inputs.Users
{
    public class UpdateUserModel
    {        
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }        
    }
}
