using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Models
{
    public sealed class User
    {
        public Guid Id { get; }
        public Surname Name { get; private set; }
        public Firstname FirstName { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public LicenceType LicenceType { get; }
        public UserType Type { get; }

        private readonly List<Lesson> _lessonsAsTeacher;
        public IReadOnlyList<Lesson> LessonsAsTeacher => _lessonsAsTeacher;

        private readonly List<Lesson> _lessonsAsStudent;
        public IReadOnlyList<Lesson> LessonsAsStudent => _lessonsAsStudent;

        private readonly List<Lesson> _waitingList;
        public IReadOnlyList<Lesson> WaitingList => _waitingList;

        private User() { }

        public User(string name, string firstName, string email, string password, LicenceType licenceType, UserType type)
        {
            Name = new Surname(name);
            FirstName = new Firstname(firstName);
            Email = new Email(email);
            Password = new Password(password);
            LicenceType = licenceType;
            Type = type;
            _lessonsAsTeacher = [];
            _lessonsAsStudent = [];
            _waitingList = [];
        }

        public User(Guid id, string name, string firstName, string email, string password, LicenceType licenceType, UserType type)
        {         
            Id = id;
            Name = new Surname(name);
            FirstName = new Firstname(firstName);
            Email = new Email(email);
            Password = new Password(password);
            LicenceType = licenceType;
            Type = type;
            _lessonsAsTeacher = [];
            _lessonsAsStudent = [];
            _waitingList = [];
        }

        public void Update(string name, string firstName, string email)
        {                        
            Name = new Surname(name);
            FirstName = new Firstname(firstName);
            Email = new Email(email);            
        }         
    }
}
