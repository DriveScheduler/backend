using Domain.Exceptions.Users;

using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed record Email
    {
        private const string REGEX_EXPR = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";        
        public string Value { get; }

        public Email(string value)
        {
            if (Regex.IsMatch(value, REGEX_EXPR) == false)
                throw new InvalidEmailException(value);
            Value = value;
        }

    }
}
