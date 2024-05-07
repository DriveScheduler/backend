using Domain.Entities;
using Domain.Exceptions.Vehicles;

using FluentValidation;

using System.Text.RegularExpressions;

namespace Domain.Validators.Vehicles
{
    public sealed class VehicleValidator : CustomValidator<Vehicle, VehicleValidationException>
    {

        public VehicleValidator()
        {
            RuleFor(vehicle => vehicle.RegistrationNumber)                                
                .Custom( (registration, context) =>
                {                                            
                    if(Regex.IsMatch(registration, @"^[A-Z]{2}\d{3}[A-Z]{2}$") == false)
                        context.AddFailure("L'immatriculation ne respecte pas le format XX123XX");
                });

            RuleFor(vehicle => vehicle.Name)
                .NotEmpty()
                .WithMessage("Le nom est obligatoire");
        }

    }
}
