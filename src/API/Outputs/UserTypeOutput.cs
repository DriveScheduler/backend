using Domain.Enums;

namespace API.Outputs
{
    public sealed class UserTypeOutput
    {
        public int Value { get; }
        public string Label { get; }

        public UserTypeOutput(UserType userType)
        {
            Value = (int)userType;
            Label = userType.ToString();
        }
    }
}
