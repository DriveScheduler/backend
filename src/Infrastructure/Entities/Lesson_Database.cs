using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Entities
{
    internal class Lesson_Database : DataEntity<Lesson>
    {      
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }        
        public int Duration { get; set; }
        public Guid TeacherId { get; set; }
        public User_Database Teacher { get; set; }
        public LicenceType Type { get; set; }
        public Vehicle_Database Vehicle { get; set; }
        public User_Database? Student { get; set; }
        public List<User_Database> WaitingList { get; set; }

        public Lesson_Database(Lesson domainModel) : base(domainModel)
        {
        }

        public override void FromDomainModel(Lesson domainModel)
        {
            Id = domainModel.Id;
            Name = domainModel.Name;
            Start = domainModel.Start;
            Duration = domainModel.Duration;
            TeacherId = domainModel.TeacherId;
            Teacher = new User_Database(domainModel.Teacher);
            Type = domainModel.Type;
            Vehicle = new Vehicle_Database(domainModel.Vehicle);
            Student = domainModel.Student is null ? null : new User_Database(domainModel.Student);
            WaitingList = domainModel.WaitingList.Select(user => new User_Database(user)).ToList();
        }

        public override Lesson ToDomainModel()
        {
            return new Lesson()
            {
                Id = Id,
                Name = Name,
                Start = Start,
                Duration = Duration,
                TeacherId = TeacherId,
                Teacher = Teacher.ToDomainModel(),
                Type = Type,
                Vehicle = Vehicle.ToDomainModel(),
                Student = Student is null ? null : Student.ToDomainModel(),
                WaitingList = WaitingList.Select(user => user.ToDomainModel()).ToList()
            };
        }
    }
}
