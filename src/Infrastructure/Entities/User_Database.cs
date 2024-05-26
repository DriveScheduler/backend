﻿using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    internal class User_Database : DataEntity<User>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public LicenceType LicenceType { get; set; }
        public UserType Type { get; set; }

        public List<Lesson_Database> LessonsAsTeacher { get; set; }
        public List<Lesson_Database> LessonsAsStudent { get; set; }
        public List<Lesson_Database> WaitingList { get; set; }

        public User_Database(User user) : base(user)
        {           
        }

        public override void FromDomainModel(User domainModel)
        {
            Id = domainModel.Id;
            Name = domainModel.Name;
            FirstName = domainModel.FirstName;
            Email = domainModel.Email;
            Password = domainModel.Password;
            LicenceType = domainModel.LicenceType;
            Type = domainModel.Type;
            LessonsAsTeacher = domainModel.LessonsAsTeacher.Select(lesson => new Lesson_Database(lesson)).ToList();
            LessonsAsStudent = domainModel.LessonsAsStudent.Select(lesson => new Lesson_Database(lesson)).ToList();
            WaitingList = domainModel.WaitingList.Select(lesson => new Lesson_Database(lesson)).ToList();
        }

        public override User ToDomainModel()
        {
            return new User()
            {
                Id = Id,
                Name = Name,
                FirstName = FirstName,
                Email = Email,
                Password = Password,
                LicenceType = LicenceType,
                Type = Type,
                LessonsAsTeacher = LessonsAsTeacher.Select(lesson => lesson.ToDomainModel()).ToList(),
            };
        }
    }
}