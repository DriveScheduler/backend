using Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configurations.ValueObjectsConverter
{
    internal class PasswordConverter : ValueConverter<Password, string>
    {
        public PasswordConverter()
            : base(email => email.Value, value => new Password (value)) { }
    }
}
