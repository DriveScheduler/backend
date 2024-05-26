namespace Infrastructure.Entities
{
    internal abstract class DataEntity<TDomainModel>
    {
        public DataEntity(TDomainModel domainModel)
        {
            FromDomainModel(domainModel);
        }
        public abstract void FromDomainModel(TDomainModel domainModel);
        public abstract TDomainModel ToDomainModel();
    }
}
