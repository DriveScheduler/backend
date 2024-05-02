using Domain.Enums;

namespace Domain.Entities
{
    public sealed class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => Start.AddMinutes(Duration);
        public int Duration { get; set; }
        public Teacher Teacher { get; set; }
        public LicenceType Type { get; set; }
        public Vehicle Vehicle { get; set; }
        public int MaxStudent { get; set; }

        public List<Student> Students { get; set; }
        public List<Student> WaitingList { get; set; }

    }
}
