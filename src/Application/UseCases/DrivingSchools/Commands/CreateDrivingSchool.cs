using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Enums;
using Domain.Exceptions.DrivingSchools;
using Domain.Validators.DrivingSchools;
using MediatR;

namespace Application.UseCases.DrivingSchools.Commands
{
    public sealed record CreateDrivingSchool_Command(string Name, string Address) : IRequest<int>;

    internal sealed class CreateDrivingSchool_CommandHandler (IDatabase database) : IRequestHandler<CreateDrivingSchool_Command, int>
    {
        private readonly IDatabase _database = database;

        public async Task<int> Handle(CreateDrivingSchool_Command request, CancellationToken cancellationToken)
        {
            
            DrivingSchool drivingSchool = new DrivingSchool()
            {
                Name = request.Name,
                Address = request.Address
            };

            new DrivingSchoolValidator(_database).ThrowIfInvalid(drivingSchool);
            
            _database.DrivingSchools.Add(drivingSchool);
            if (await _database.SaveChangesAsync() != 1)
                throw new DrivingSchoolSaveException();

            return drivingSchool.Id;
        }
    }
}