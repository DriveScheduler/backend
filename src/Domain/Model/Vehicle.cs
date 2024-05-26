using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public sealed class Vehicle
    {
        public int Id { get; }
        public RegistrationNumber RegistrationNumber { get; private set; }
        public string Name { get; private set; }
        public LicenceType Type { get; }

        public readonly List<Lesson> _lessons;
        public IReadOnlyList<Lesson> Lessons => _lessons;

        public Vehicle(string registrationNumber, string name, LicenceType type)
        {            
            RegistrationNumber = new RegistrationNumber(registrationNumber);
            Name = name;
            Type = type;
            _lessons = [];
        }

        public Vehicle(int id, string registrationNumber, string name, LicenceType type)
        {
            Id = id;
            RegistrationNumber = new RegistrationNumber(registrationNumber);
            Name = name;
            Type = type;
            _lessons = [];
        }

        public void Update(string registrationNumber, string name)
        {
            //RegistrationNumber = new RegistrationNumber(registrationNumber);
            //Name = name;
            throw new NotImplementedException();
        }
    }
}
