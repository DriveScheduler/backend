using Domain.Exceptions.Users;

namespace Domain.ValueObjects
{
    public sealed record Password
    {
        public string Value { get; }

        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new UserValidationException("Le mot de passe est obligatoire");          
            Value = value;
        }
    }
}
