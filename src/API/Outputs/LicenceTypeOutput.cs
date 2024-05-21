using Domain.Enums;

namespace API.Outputs
{
    public sealed class LicenceTypeOutput
    {
        public int Value { get; }
        public string Label { get; }

        public LicenceTypeOutput(LicenceType licenceType)
        {
            Value = (int)licenceType;
            Label = licenceType.ToText();
        }
    }
}
