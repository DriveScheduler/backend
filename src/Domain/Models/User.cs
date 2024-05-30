﻿using Domain.Enums;
using Domain.Exceptions.Users;
using Domain.ValueObjects;

namespace Domain.Models
{
    public sealed class User
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string FirstName { get; private set; }
        public Email Email { get; private set; }
        public string Password { get; private set; }
        public LicenceType LicenceType { get; }
        public UserType Type { get; }

        private readonly List<Lesson> _lessonsAsTeacher;
        public IReadOnlyList<Lesson> LessonsAsTeacher => _lessonsAsTeacher;

        private readonly List<Lesson> _lessonsAsStudent;
        public IReadOnlyList<Lesson> LessonsAsStudent => _lessonsAsStudent;

        private readonly List<Lesson> _waitingList;
        public IReadOnlyList<Lesson> WaitingList => _waitingList;

        private User() { }

        public User(string name, string firstName, string email, string password, LicenceType licenceType, UserType type)
        {
            ValidateNameOrThrow(name);
            ValidateFirstNameOrThrow(firstName);
            ValidatePasswordOrThrow(password);            
            Name = name;
            FirstName = firstName;
            Email = new Email(email);
            Password = password;
            LicenceType = licenceType;
            Type = type;
            _lessonsAsTeacher = [];
            _lessonsAsStudent = [];
            _waitingList = [];
        }

        public User(Guid id, string name, string firstName, string email, string password, LicenceType licenceType, UserType type)
        {
            ValidateNameOrThrow(name);
            ValidateFirstNameOrThrow(firstName);            
            ValidatePasswordOrThrow(password);
            Id = id;
            Name = name;
            FirstName = firstName;
            Email = new Email(email);
            Password = password;
            LicenceType = licenceType;
            Type = type;
            _lessonsAsTeacher = [];
            _lessonsAsStudent = [];
            _waitingList = [];
        }

        public void Update(string name, string firstName, string email)
        {
            ValidateNameOrThrow(name);
            ValidateFirstNameOrThrow(firstName);                        
            Name = name;
            FirstName = firstName;
            Email = new Email(email);            
        }

        private void ValidatePasswordOrThrow(string password)
        {
            if(string.IsNullOrWhiteSpace(password))
                throw new UserValidationException("Le mot de passe est obligatoire");
        }       

        private void ValidateFirstNameOrThrow(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new UserValidationException("Le prénom est obligatoire");
        }

        private void ValidateNameOrThrow(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new UserValidationException("Le nom est obligatoire");
        }
    }
}
