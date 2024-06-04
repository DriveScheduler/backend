using Domain.Enums;

namespace Domain.Models.Users
{
    public sealed class Admin : User
    {
        public Admin(string name, string firstName, string email, string password, LicenceType licenceType) : 
            base(name, firstName, email, password, licenceType)
        {
        }

        public Admin(Guid id, string name, string firstName, string email, string password, LicenceType licenceType) : 
            base(id, name, firstName, email, password, licenceType)
        {
        }

        public override UserType GetRole() => UserType.Admin;      
    }
}
