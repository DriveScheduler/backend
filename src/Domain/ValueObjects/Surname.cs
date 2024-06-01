using Domain.Exceptions.Users;

namespace Domain.ValueObjects
{
    public sealed record Surname
    {
        public string Value { get; }

        public Surname(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new UserValidationException("Le nom est obligatoire");
            Value = value.Trim().ToUpper();
        }
    }
}
