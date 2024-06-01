using Domain.Exceptions.Users;

namespace Domain.ValueObjects
{
    public sealed record Firstname
    {
        public string Value { get; }

        public Firstname(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new UserValidationException("Le prénom est obligatoire");
            string trimmedValue = value.Trim();

            if (trimmedValue.Length < 2) Value = trimmedValue.ToUpper();
            Value = char.ToUpper(trimmedValue[0]) + trimmedValue[1..].ToLower();
        }
    }
}
