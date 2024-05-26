using Domain.Exceptions.Vehicles;

using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed record RegistrationNumber
    {
        private const string REGEX_EXPR = @"^[A-Z]{2}\d{3}[A-Z]{2}$";

        public string Value { get; }

        public RegistrationNumber(string value)
        {
            if (Regex.IsMatch(value, REGEX_EXPR) == false)
                throw new RegistrationNumberException();
            Value = value;
        }

    }
}
