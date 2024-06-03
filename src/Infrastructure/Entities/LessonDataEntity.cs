using Domain.Enums;
using Domain.Models;
using Domain.Models.Users;

namespace Infrastructure.Entities
{
    internal sealed class LessonDataEntity : DataEntity<Lesson>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public LicenceType Type { get; set; }

        public Guid TeacherId { get; set; }
        public UserDataEntity Teacher { get; set; }

        public int VehicleId { get; set; }
        public VehicleDataEntity Vehicle { get; set; }

        public Guid? StudentId { get; set; }
        public UserDataEntity? Student { get; set; }

        public List<Guid> UserWaitingListId { get; set; } = [];
        public List<UserDataEntity> UserWaitingList { get; set; }

        private LessonDataEntity() { }

        public LessonDataEntity(Lesson domainModel) : base(domainModel) { }

        public override void FromDomainModel(Lesson domainModel)
        {
            Id = domainModel.Id;
            Name = domainModel.Name;
            Start = domainModel.Start;
            Duration = domainModel.Duration.Value;
            TeacherId = domainModel.Teacher.Id;            
            Type = domainModel.Type;
            VehicleId = domainModel.Vehicle.Id;            
            StudentId = domainModel.Student == null ? null : domainModel.Student.Id;            
            UserWaitingListId = domainModel.WaitingList.Select(u => u.Id).ToList();            
        }

        public override Lesson ToDomainModel(int level)
        {
            if(level >= 2) return null;
            level++;

            Student? student = Student == null ? null : (Student)Student.ToDomainModel(level);            
            Lesson lesson = new Lesson(Id, Name, Start, Duration, (Teacher)Teacher.ToDomainModel(level), Type, Vehicle.ToDomainModel(level), student);
            SetPrivateField(lesson, "_waitingList", UserWaitingList.Select(u => (Student)u.ToDomainModel(level)).ToList());

            return lesson;
        }
    }
}
