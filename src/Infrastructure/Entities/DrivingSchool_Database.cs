using Domain.Entities;

namespace Infrastructure.Entities
{
    internal class DrivingSchool_Database : DataEntity<DrivingSchool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public DrivingSchool_Database(DrivingSchool domainModel) : base(domainModel)
        {
        }

        public override void FromDomainModel(DrivingSchool domainModel)
        {
            Id = domainModel.Id;
            Name = domainModel.Name;
            Address = domainModel.Address;
        }

        public override DrivingSchool ToDomainModel()
        {
            return new DrivingSchool(Id, Name, Address);            
        }
    }
}
