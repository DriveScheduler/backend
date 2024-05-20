using Domain.Abstractions;
using Domain.Entities.Database;
using Domain.Enums;
using Domain.Exceptions.DrivingSchools;
using Domain.Validators.DrivingSchools;
using MediatR;

namespace Application.UseCases.DrivingSchools.Commands
{
    public sealed record UpdateDrivingSchool_Command(int Id, string Name, string Address) : IRequest;

    internal sealed class UpdateDrivingSchool_CommandHandler (IDatabase database) : IRequestHandler<UpdateDrivingSchool_Command>
    {
        private readonly IDatabase _database = database;

        public async Task Handle(UpdateDrivingSchool_Command request, CancellationToken cancellationToken)
        {
            DrivingSchool? drivingSchool = _database.DrivingSchools.Find(request.Id);
            if (drivingSchool is null)
                throw new DrivingSchoolNotFoundException();

            drivingSchool.Name = request.Name;
            drivingSchool.Address = request.Address;

            new DrivingSchoolValidator(_database).ThrowIfInvalid(drivingSchool);


            if (await _database.SaveChangesAsync() != 1)
                throw new DrivingSchoolSaveException();
        }
    }
}