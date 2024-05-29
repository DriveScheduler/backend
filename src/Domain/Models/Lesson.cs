using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.ValueObjects;

namespace Domain.Models
{
    public sealed class Lesson
    {
        public int Id { get; }
        public string Name { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End => Start.AddMinutes(Duration.Value);
        public LessonDuration Duration { get; private set; }  
        public Guid TeacherId { get; private set; }
        public User Teacher { get; private set; }
        public LicenceType Type { get; set; }
        public Vehicle Vehicle { get; private set; }
        public User? Student { get; private set; }
        
        private readonly List<User> _waitingList;
        public IReadOnlyList<User> WaitingList => _waitingList;

        private Lesson() { }

        public Lesson(string name, DateTime start, int duration, User teacher, LicenceType type, Vehicle vehicle)
        {            
            Name = name;
            Start = start;
            Duration = new LessonDuration(duration);
            Teacher = teacher;
            Type = type;
            Vehicle = vehicle;
            Student = null;
            _waitingList = [];
        }
        public Lesson(int id, string name, DateTime start, int duration, User teacher, LicenceType type, Vehicle vehicle, User? student=null)
        {
            Id = id;
            Name = name;
            Start = start;
            Duration = new LessonDuration(duration);
            Teacher = teacher;
            Type = type;
            Vehicle = vehicle;
            Student = student;
            _waitingList = [];
        }

        public void Update(string name, DateTime start, int duration, User teacher, Vehicle vehicle)
        {
            Name = name;
            Start = start;
            Duration = new LessonDuration(duration);
            Teacher = teacher;
            Vehicle = vehicle;
        }

        public void AddStudent(User student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            if (Student is not null) throw new LessonFullException();
            ThrowIfLicenceTypeNotMatch(student);
            ThrowIfUserIsNotStudent(student);

            Student = student;            
        }
        public void RemoveStudent(User student)
        {
            //if (student is null) throw new ArgumentNullException(nameof(student));
            //if (Student is null) throw new LessonValidationException("Student is not already set");

            //if (Student.Id != student.Id) throw new LessonValidationException("Student is not the same");

            //Student = null;
            throw new NotImplementedException();
        }

        public void AddStudentToWaitingList(User student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            if (Student is null) throw new LessonValidationException("Le cours n'est pas complet");
            ThrowIfLicenceTypeNotMatch(student);
            ThrowIfUserIsNotStudent(student, "L'utilisateur doit être un élève pour s'incrire à la file d'attente du cours");

            if (_waitingList.Any(user => user.Id == student.Id))
                throw new LessonValidationException("L'utilisateur est déjà dans la liste d'attente");

            //if (Student.Id != student.Id) throw new LessonValidationException("Student is not the same");

            //if(_waitingList.Contains(student)) throw new LessonValidationException("Student is already in the waiting list");
            
            _waitingList.Add(student);
        }

        public void RemoveStudentFromWaitingList(User student)
        {
            //if (student is null) throw new ArgumentNullException(nameof(student));
            //if (Student is null) throw new LessonValidationException("Student is not already set");

            //if (Student.Id != student.Id) throw new LessonValidationException("Student is not the same");

            //if (_waitingList.Contains(student)) throw new LessonValidationException("Student is already in the waiting list");

            //_waitingList.Add(student);
            throw new NotImplementedException();
        }


        public UserLessonState State(User user)
        {
            if (user is null || Student is null) return UserLessonState.Free;

            if (WaitingList.Contains(user)) return UserLessonState.InWaitingList;

            if (Student.Id == user.Id) return UserLessonState.BookedByUser;
            else return UserLessonState.BookedByOther;
        }

        private void ThrowIfLicenceTypeNotMatch(User user)
        {
            if (user.LicenceType != Type)
                throw new LessonValidationException("Le permis de l'utilisateur ne correspond pas au type de cours");
        }

        private void ThrowIfUserIsNotStudent(User user, string? message=null)
        {
            if (user.Type != UserType.Student)
                throw new LessonValidationException(
                    message is null ? "L'utilisateur doit être un élève pour s'incrire au cours" : message);
        }

    }
}
