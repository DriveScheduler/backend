using Domain.Enums;

namespace API.Output
{
    public sealed class UserTypeOutput
    {
        public int Value { get; }
        public string Label { get; }

        public UserTypeOutput(UserType userType)
        {
            Value = (int)userType;
            Label = userType.ToText();
        }
    }
}
