namespace Domain.Enums
{
    public enum LicenceType
    {
        Car,
        Truck,
        Motorcycle,
        Bus
    }

    public static class LicenceTypeExtension
    {
        public static string ToText(this LicenceType licenceType) =>
            licenceType switch
            {
                LicenceType.Car => "B - Voiture",
                LicenceType.Truck => "C - Poids lourd",
                LicenceType.Motorcycle => "A2 - Moto",
                LicenceType.Bus => "D - Bus",
                _ => string.Empty
            };
    }
}
