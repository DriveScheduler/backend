using FluentValidation;

using System.Reflection;

namespace Domain.Validators
{
    public abstract class CustomValidator<TModel, TException> : AbstractValidator<TModel>
        where TModel : class
        where TException : Exception
    {
        protected TModel? InitialState { get; init; }

        public CustomValidator() => InitialState = null;
        public CustomValidator(TModel initialState) => InitialState = Clone(initialState);

        public void ThrowIfInvalid(TModel model)
        {
            var results = Validate(model);
            if (!results.IsValid)
            {
                throw (Exception)Activator.CreateInstance(typeof(TException), new[] { results.Errors[0].ErrorMessage })!;
            }
        }

        private TModel Clone(TModel model)
        {
            Type t = model.GetType();
            var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            TModel copy = (TModel)Activator.CreateInstance(t)!;
            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(copy, fields[i].GetValue(model));

            return copy!;
        }
    }
}
