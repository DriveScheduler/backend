using Domain.Enums;

namespace API.Output
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
