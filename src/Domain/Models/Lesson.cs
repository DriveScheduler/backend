using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models.Users;
using Domain.Models.Vehicles;
using Domain.ValueObjects;

namespace Domain.Models
{
    public sealed class Lesson : IEquatable<Lesson>
    {
        public int Id { get; }
        public string Name { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End => Start.AddMinutes(Duration.Value);
        public LessonDuration Duration { get; private set; }        
        public Teacher Teacher { get; private set; }
        public LicenceType Type { get; set; }
        public Vehicle Vehicle { get; private set; }
        public Student? Student { get; private set; }

        private readonly List<Student> _waitingList;
        public IReadOnlyList<Student> WaitingList => _waitingList;

        private Lesson() { }

        public Lesson(string name, DateTime start, int duration, Teacher teacher, LicenceType type, Vehicle vehicle)
        {
            ThrowIfInvalidName(name);            
            ThrowIfLicenceTypeNotMatch(teacher, "Le moniteur doit pouvoir assurer ce type de cours");

            Name = name;
            Start = start;
            Duration = new LessonDuration(duration);
            Teacher = teacher;
            Type = type;
            Vehicle = vehicle;
            Student = null;
            _waitingList = [];
        }
        public Lesson(int id, string name, DateTime start, int duration, Teacher teacher, LicenceType type, Vehicle vehicle, Student? student = null)
        {
            ThrowIfInvalidName(name);            
            ThrowIfLicenceTypeNotMatch(teacher, "Le moniteur doit pouvoir assurer ce type de cours");

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

        public void Update(string name, DateTime start, int duration, Teacher teacher, Vehicle vehicle)
        {
            ThrowIfInvalidName(name);            
            ThrowIfLicenceTypeNotMatch(teacher, "Le moniteur doit pouvoir assurer ce type de cours");

            Name = name;
            Start = start;
            Duration = new LessonDuration(duration);
            Teacher = teacher;
            Vehicle = vehicle;
        }

        public void AddStudent(Student student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            if (Student is not null) throw new LessonValidationException("Le cours est complet");
            ThrowIfLicenceTypeNotMatch(student);            

            Student = student;
        }
        public void RemoveStudent()
        {
            Student = null;
        }

        public void AddStudentToWaitingList(Student student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            if (Student is null) throw new LessonValidationException("Le cours n'est pas complet");
            ThrowIfLicenceTypeNotMatch(student);            

            if (_waitingList.Any(user => user.Id == student.Id))
                throw new LessonValidationException("L'utilisateur est déjà dans la liste d'attente");        

            if(_waitingList.Contains(student)) return;

            _waitingList.Add(student);
        }

        public void RemoveStudentFromWaitingList(Student student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            Student? studentInWaitingList = _waitingList.FirstOrDefault(s => s.Id == student.Id);
            if (studentInWaitingList is not null)
                _waitingList.Remove(studentInWaitingList);
        }


        public UserLessonState State(User user)
        {
            if (user is null || Student is null) return UserLessonState.Free;

            if (WaitingList.Contains(user)) return UserLessonState.InWaitingList;

            if (Student.Id == user.Id) return UserLessonState.BookedByUser;
            else return UserLessonState.BookedByOther;
        }       

        private void ThrowIfInvalidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new LessonValidationException("Le nom du cours est obligatoire");
        }

        private void ThrowIfLicenceTypeNotMatch(User user, string? message = null)
        {
            if (user is null) return;
            if (user.LicenceType != Type)
                throw new LessonValidationException(
                    message is null ? "Le permis de l'utilisateur ne correspond pas au type de cours" : message);
        }


        public override bool Equals(object? obj)
        {
            if (obj is Lesson lesson)
                return Id == lesson.Id;
            return false;
        }

        public bool Equals(Lesson? other)
        {
            return Equals(other as object);
        }

        public override int GetHashCode()
        {
            return Id;
        }

     
    }
}
