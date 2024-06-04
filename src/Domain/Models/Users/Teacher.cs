using Domain.Enums;

namespace Domain.Models.Users
{
    public sealed class Teacher : User
    {        
        private readonly List<Lesson> _lessons;
        public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

        public Teacher(string name, string firstName, string email, string password, LicenceType licenceType) :
            base(name, firstName, email, password, licenceType)
        {
            _lessons = [];
        }

        public Teacher(Guid id, string name, string firstName, string email, string password, LicenceType licenceType) :
            base(id, name, firstName, email, password, licenceType)
        {
            _lessons = [];
        }

        public override UserType GetRole() => UserType.Teacher;

    }
}
