using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string FirstName { get; set; }
        public required string Email { get; set; } 
        public required string Password { get; set; }
        public required LicenceType LicenceType { get; set; }
        public required UserType Type { get; set; }
        
        public List<Lesson> LessonsAsTeacher { get; set; }        
        public List<Lesson> LessonsAsStudent { get; set; }        
        public List<Lesson> WaitingList { get; set; }
    }
}
