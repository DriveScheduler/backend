using FluentValidation;

namespace Domain.Validators
{
    public class CustomValidator<TModel, TException> : AbstractValidator<TModel>
        where TModel : class
        where TException : Exception         
    {
        public void ThrowIfInvalid(TModel model)            
        {
            var results = Validate(model);
            if (!results.IsValid)
            {
                throw (Exception)Activator.CreateInstance(typeof(TException), new[] { results.Errors[0].ErrorMessage })!;
            }
        }
    }
}
