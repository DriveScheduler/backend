using Domain.Enums;
using Domain.Models.Users;

namespace Infrastructure.Entities
{
    internal sealed class UserDataEntity : DataEntity<User>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public LicenceType LicenceType { get; set; }
        public UserType Type { get; set; }

        public List<int> LessonsAsTeacherId { get; set; } = [];
        public List<LessonDataEntity> LessonsAsTeacher { get; set; }

        public List<int> LessonsAsStudentId { get; set; } = [];
        public List<LessonDataEntity> LessonsAsStudent { get; set; }

        public List<int> LessonWaitingListId { get; set; } = [];
        public List<LessonDataEntity> LessonWaitingList { get; set; }

        private UserDataEntity() { }
        public UserDataEntity(User domainModel) : base(domainModel) { }

        public override void FromDomainModel(User domainModel)
        {
            Id = domainModel.Id;
            Name = domainModel.Name.Value;
            FirstName = domainModel.FirstName.Value;
            Email = domainModel.Email.Value;
            Password = domainModel.Password.Value;
            LicenceType = domainModel.LicenceType;
            Type = domainModel.GetRole();
            if (domainModel is Student student)
            {
                LessonsAsStudentId = student.Lessons.Select(l => l.Id).ToList();                
                LessonWaitingListId = student.WaitingList.Select(l => l.Id).ToList();               
            }
            else if (domainModel is Teacher teacher)
            {
                LessonsAsTeacherId = teacher.Lessons.Select(l => l.Id).ToList();                
            }
        }

        public override User ToDomainModel(int level)
        {
            if(level >= 2) return null;
            level++;

            User user;
            if (Type == UserType.Student)
            {
                user = new Student(Id, Name, FirstName, Email, Password, LicenceType);
                SetPrivateField((Student)user, "_lessons", LessonsAsStudent.Select(l => l.ToDomainModel(level)).ToList());
                SetPrivateField((Student)user, "_waitingList", LessonWaitingList.Select(l => l.ToDomainModel(level)).ToList());
            }
            else if (Type == UserType.Teacher)
            {
                user = new Teacher(Id, Name, FirstName, Email, Password, LicenceType);
                SetPrivateField((Teacher)user, "_lessons", LessonsAsTeacher.Select(l => l.ToDomainModel(level)).ToList());
            }
            else
            {
                user = new Admin(Name, FirstName, Email, Password, LicenceType);
            }
            return user;
        }
    }
}
