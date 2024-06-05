using System.Reflection;

namespace Infrastructure.Entities
{
    internal abstract class DataEntity<TDomainModel> where TDomainModel : class
    {
        public DataEntity() { }

        public DataEntity(TDomainModel domainModel)
        {
            FromDomainModel(domainModel);
        }

        public abstract TDomainModel BaseDomainModel();
        public abstract TDomainModel FullDomainModel();
        public abstract void FromDomainModel(TDomainModel domainModel);

        protected static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {            
            typeof(T)
               .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
               ?.SetValue(entity, value);
        }
    }
}
