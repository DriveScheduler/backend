﻿using Domain.Enums;
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

        //public List<Guid> UserWaitingListId { get; set; } = [];
        //public List<UserDataEntity> UserWaitingList { get; set; }
        public List<UserLessonWaitingList> UserWaitingLists { get; set; }

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
            StudentId = domainModel.Student?.Id;
            //UserWaitingListId = domainModel.WaitingList.Select(u => u.Id).ToList();
            UserWaitingLists = domainModel.WaitingList.Select(u => new UserLessonWaitingList() { UserId = u.Id, LessonId = domainModel.Id}).ToList();
        }

        public override Lesson ToDomainModel()
        {
            Student? student = Student == null ? null : (Student)Student.ToDomainModel();
            Lesson lesson = new Lesson(Id, Name, Start, Duration, (Teacher)Teacher.ToDomainModel(), Type, Vehicle.ToDomainModel(), student);            

            return lesson;
        }

        public override Lesson ToDomainModel_Deep()
        {
            Lesson lesson = ToDomainModel();
            SetPrivateField(lesson, "_waitingList", UserWaitingLists.Select(u => (Student)u.User.ToDomainModel()).ToList());
            return lesson;
        }
    }
}
