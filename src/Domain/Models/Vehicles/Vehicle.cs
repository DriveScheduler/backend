using Domain.Enums;
using Domain.Exceptions.Vehicles;
using Domain.ValueObjects;

namespace Domain.Models.Vehicles
{
    public abstract class Vehicle
    {
        public int Id { get; }
        public RegistrationNumber RegistrationNumber { get; private set; }
        public string Name { get; private set; }        


        public readonly List<Lesson> _lessons;
        public IReadOnlyList<Lesson> Lessons => _lessons;

        public abstract LicenceType GetType();        

        public Vehicle(string registrationNumber, string name)
        {
            ThrowIfInvalidName(name);

            RegistrationNumber = new RegistrationNumber(registrationNumber);
            Name = name;            
            _lessons = [];
        }

        public Vehicle(int id, string registrationNumber, string name)
        {
            ThrowIfInvalidName(name);

            Id = id;
            RegistrationNumber = new RegistrationNumber(registrationNumber);
            Name = name;            
            _lessons = [];
        }

        public void Update(string registrationNumber, string name)
        {
            ThrowIfInvalidName(name);

            RegistrationNumber = new RegistrationNumber(registrationNumber);
            Name = name;
        }

        private void ThrowIfInvalidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new VehicleValidationException("Le nom est obligatoire");
        }
    }
}
