using Domain.Models;
using Domain.Enums;

namespace Infrastructure.Entities
{
    internal class Vehicle_Database : DataEntity<Vehicle>
    {     
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public LicenceType Type { get; set; }
        public List<Lesson_Database> Lessons { get; set; }

        public Vehicle_Database(Vehicle domainModel) : base(domainModel)
        {
        }

        public override void FromDomainModel(Vehicle domainModel)
        {
            Id = domainModel.Id;
            RegistrationNumber = domainModel.RegistrationNumber.Value;
            Name = domainModel.Name;
            Type = domainModel.Type;
            Lessons = domainModel.Lessons.Select(lesson => new Lesson_Database(lesson)).ToList();
        }

        public override Vehicle ToDomainModel()
        {
            return new Vehicle(Id, RegistrationNumber, Name, Type);          
        }
    }
}
