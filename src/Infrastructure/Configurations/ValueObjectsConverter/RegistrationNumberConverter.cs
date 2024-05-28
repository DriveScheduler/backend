using Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configurations.ValueObjectsConverter
{
    internal class RegistrationNumberConverter : ValueConverter<RegistrationNumber, string>
    {
        public RegistrationNumberConverter()
            : base(registrationNumber => registrationNumber.Value, value => new RegistrationNumber(value)) { }
    }   
}
