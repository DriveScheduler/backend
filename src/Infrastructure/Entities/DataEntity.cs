using System.Reflection;

namespace Infrastructure.Entities
{
    public abstract class DataEntity<TDomainModel> where TDomainModel : class
    {
        public DataEntity() { }

        public DataEntity(TDomainModel domainModel)
        {
            FromDomainModel(domainModel);
        }

        public abstract TDomainModel ToDomainModel(int level);
        public abstract void FromDomainModel(TDomainModel domainModel);

        protected static void SetPrivateField<T>(T entity, string fieldName, object value) where T : class
        {
            var field = typeof(T).GetField($"<{fieldName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(entity, value);
        }
    }
}
