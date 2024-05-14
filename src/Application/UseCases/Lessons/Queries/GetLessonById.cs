using Domain.Abstractions;
using Domain.Entities.Database;
using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetLessonById_Query(int Id) : IRequest<Lesson>;

    internal sealed class GetLessonById_QueryHandler(IDatabase database) : IRequestHandler<GetLessonById_Query, Lesson>
    {
        private readonly IDatabase _database = database;

        public Task<Lesson> Handle(GetLessonById_Query request, CancellationToken cancellationToken)
        {
            Lesson? user = _database.Lessons.FirstOrDefault(x => x.Id == request.Id);
            if (user is null)
                throw new Exception();

            return Task.FromResult(user);
        }
    }
}
