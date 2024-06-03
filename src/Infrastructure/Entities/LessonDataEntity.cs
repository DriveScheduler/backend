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

        public List<Guid> WaitingListId { get; set; } = [];
        public List<UserDataEntity> WaitingList { get; set; }

        public LessonDataEntity() { }

        public LessonDataEntity(Lesson domainModel) : base(domainModel) { }

        public override void FromDomainModel(Lesson domainModel)
        {
            Id = domainModel.Id;
            Name = domainModel.Name;
            Start = domainModel.Start;
            Duration = domainModel.Duration.Value;
            TeacherId = domainModel.Teacher.Id;
            //Teacher = new UserDataEntity(domainModel.Teacher);
            Type = domainModel.Type;
            VehicleId = domainModel.Vehicle.Id;
            //Vehicle = new VehicleDataEntity(domainModel.Vehicle);
            StudentId = domainModel.Student == null ? null : domainModel.Student.Id;
            //Student = domainModel.Student == null ? null : new UserDataEntity(domainModel.Student);
            WaitingListId = domainModel.WaitingList.Select(u => u.Id).ToList();
            //WaitingList = domainModel.WaitingList.Select(u => new UserDataEntity(u)).ToList();
        }

        public override Lesson ToDomainModel()
        {
            Student? student = Student == null ? null : (Student)Student.ToDomainModel();
            Lesson lesson = new Lesson(Id, Name, Start, Duration, (Teacher)Teacher.ToDomainModel(), Type, Vehicle.ToDomainModel(), student);
            SetPrivateField(lesson, "_waitingList", WaitingList.Select(u => (Student)u.ToDomainModel()).ToList());
            return lesson;
        }
    }
}
