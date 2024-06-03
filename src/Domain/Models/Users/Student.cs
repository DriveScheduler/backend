using Domain.Enums;

namespace Domain.Models.Users
{
    public sealed class Student : User
    {
        private readonly List<Lesson> _lessons;
        public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

        private readonly List<Lesson> _waitingList;
        public IReadOnlyList<Lesson> WaitingList => _waitingList.AsReadOnly();

        public Student(string name, string firstName, string email, string password, LicenceType licenceType) :
            base(name, firstName, email, password, licenceType)
        {
            _lessons = [];
            _waitingList = [];
        }

        public Student(Guid id, string name, string firstName, string email, string password, LicenceType licenceType) :
           base(id, name, firstName, email, password, licenceType)
        {
            _lessons = [];
            _waitingList = [];
        }

        public override UserType GetRole() => UserType.Student;        
    }
}
