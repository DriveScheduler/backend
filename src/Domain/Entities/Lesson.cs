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
        public Guid TeacherId { get; set; }
        public User Teacher { get; set; }
        public LicenceType Type { get; set; }
        public Vehicle Vehicle { get; set; }        
        public User? Student { get; set; }
        public List<User> WaitingList { get; set; }

    }
}
