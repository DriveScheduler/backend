namespace Domain.Entities
{
    public sealed class Teacher : User
    {
        public List<Lesson> Lessons { get; set; }
    }
}
