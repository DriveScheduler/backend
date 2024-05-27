using Domain.Exceptions.DrivingSchools;

namespace Domain.Models;

public class DrivingSchool
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }

    private DrivingSchool() { }

    public DrivingSchool(string name, string address)
    {
        Create(name, address);
    }
    public DrivingSchool(int id, string name, string address)
    {
        Create(name, address);
        Id = id;
    }

    public void Update(string name, string address)
    {
        ValidateNameOrThrow(name);
        ValidateAddressOrThrow(address);
        Name = name;
        Address = address;
    }

    private void ValidateNameOrThrow(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DrivingSchoolValidationException("Le nom est obligatoire");
    }

    private void ValidateAddressOrThrow(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DrivingSchoolValidationException("L'adresse est obligatoire");
    }

    private void Create(string name, string address)
    {
        ValidateNameOrThrow(name);
        ValidateAddressOrThrow(address);
        Name = name;
        Address = address;
    }
}