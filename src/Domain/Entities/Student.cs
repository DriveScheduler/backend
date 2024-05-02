namespace Domain.Entities
{
    public sealed class Student : User
    {      
        public List<Lesson> Lessons { get; set; }
        public List<Lesson> WaitingLessons { get; set; }
    }
}
