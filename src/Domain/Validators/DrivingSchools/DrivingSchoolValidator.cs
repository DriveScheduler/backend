using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Exceptions.DrivingSchools;

using FluentValidation;

using System.Text.RegularExpressions;

namespace Domain.Validators.DrivingSchools
{
    public sealed class DrivingSchoolValidator : CustomValidator<DrivingSchool, DrivingSchoolValidationException>
    {
        public DrivingSchoolValidator(IDatabase database)
        {
            RuleFor(drivingSchool => drivingSchool.Name)
                .NotEmpty()
                .WithMessage("Le nom est obligatoire");

            RuleFor(drivingSchool => drivingSchool.Address)
                .NotEmpty()
                .WithMessage("L'adresse est obligatoire");
        }
    }
}