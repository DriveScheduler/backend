using Domain.Enums;
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
        public User Teacher { get; private set; }
        public LicenceType Type { get; set; }
        public Vehicle Vehicle { get; private set; }
        public User? Student { get; private set; }
        
        private readonly List<User> _waitingList;
        public IReadOnlyList<User> WaitingList { get; }

        public Lesson(string name, DateTime start, int duration, User teacher, LicenceType type, Vehicle vehicle)
        {            
            Name = name;
            Start = start;
            Duration = new LessonDuration(duration);
            Teacher = teacher;
            Type = type;
            Vehicle = vehicle;
            Student = null;
            WaitingList = [];
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
            WaitingList = [];
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
            //if (student is null) throw new ArgumentNullException(nameof(student));
            //if (Student is not null) throw new LessonValidationException("Student is already set");

            //Student = student;
            throw new NotImplementedException();
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
            //if (student is null) throw new ArgumentNullException(nameof(student));
            //if (Student is null) throw new LessonValidationException("Student is not already set");

            //if (Student.Id != student.Id) throw new LessonValidationException("Student is not the same");

            //if(_waitingList.Contains(student)) throw new LessonValidationException("Student is already in the waiting list");
            
            //_waitingList.Add(student);
            throw new NotImplementedException();
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

    }
}
