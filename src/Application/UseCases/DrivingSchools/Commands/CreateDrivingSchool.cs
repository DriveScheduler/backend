using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.DrivingSchools.Commands
{
    public sealed record CreateDrivingSchool_Command(string Name, string Address) : IRequest<int>;

    internal sealed class CreateDrivingSchool_CommandHandler(IDrivingSchoolRepository drivingSchoolRepository) : IRequestHandler<CreateDrivingSchool_Command, int>
    {
        private readonly IDrivingSchoolRepository _drivingSchoolRepository = drivingSchoolRepository;

        public async Task<int> Handle(CreateDrivingSchool_Command request, CancellationToken cancellationToken)
        {

            //DrivingSchool drivingSchool = new DrivingSchool()
            //{
            //    Name = request.Name,
            //    Address = request.Address
            //};

            //new DrivingSchoolValidator(_database).ThrowIfInvalid(drivingSchool);

            //_database.DrivingSchools.Add(drivingSchool);
            //if (await _database.SaveChangesAsync() != 1)
            //    throw new DrivingSchoolSaveException();

            //return drivingSchool.Id;

            DrivingSchool drivingSchool = new DrivingSchool(request.Name, request.Address);
            return await _drivingSchoolRepository.InsertAsync(drivingSchool);
        }
    }
}