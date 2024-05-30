using Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configurations.ValueObjectsConverter
{
    internal class EmailConverter : ValueConverter<Email, string>
    {
        public EmailConverter()
            : base(email => email.Value, value => new Email(value)) { }
    }
}
