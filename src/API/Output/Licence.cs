using Domain.Enums;

namespace API.Output
{
    public class Licence
    {
        public int Value { get; }
        public string Label { get; }

        public Licence(LicenceType licenceType)
        {
            Value = (int)licenceType;
            Label = licenceType.ToText();
        }
    }
}
