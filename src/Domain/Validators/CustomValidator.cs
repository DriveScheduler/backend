using FluentValidation;

namespace Domain.Validators
{
    public abstract class CustomValidator<TModel, TException> : AbstractValidator<TModel>
        where TModel : class
        where TException : Exception
    {
        private Action<TModel>? _executeBeforeThrowing { get; set; } = null;

        public void ThrowIfInvalid(TModel model)
        {
            var results = Validate(model);            
            if (!results.IsValid)
            {
                _executeBeforeThrowing?.Invoke(model);
                throw (Exception)Activator.CreateInstance(typeof(TException), new[] { results.Errors[0].ErrorMessage })!;
            }            
        }

        public CustomValidator<TModel, TException> ExecuteBeforeThrowing(Action<TModel> func)
        {
            _executeBeforeThrowing = func;
            return this;
        }
    }
}
