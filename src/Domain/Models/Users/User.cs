﻿using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Models.Users
{
    public abstract class User : IEquatable<User>
    {
        public Guid Id { get; }
        public Surname Name { get; private set; }
        public Firstname FirstName { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public LicenceType LicenceType { get; }

        public User(string name, string firstName, string email, string password, LicenceType licenceType)
        {            
            Name = new Surname(name);
            FirstName = new Firstname(firstName);
            Email = new Email(email);
            Password = new Password(password);
            LicenceType = licenceType;
        }

        public User(Guid id, string name, string firstName, string email, string password, LicenceType licenceType)
        {
            Id = id;
            Name = new Surname(name);
            FirstName = new Firstname(firstName);
            Email = new Email(email);
            Password = new Password(password);
            LicenceType = licenceType;
        }
        public void Update(string name, string firstName, string email)
        {
            Name = new Surname(name);
            FirstName = new Firstname(firstName);
            Email = new Email(email);
        }

        public abstract UserType GetRole();

        public override bool Equals(object? obj)
        {
            if (obj is User user)
                return Id == user.Id;
            return false;
        }

        public bool Equals(User? other)
        {
            return Equals(other as object);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
