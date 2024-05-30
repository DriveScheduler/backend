using Domain.Models;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Users.Queries
{
    public sealed record GetLessonById_Query(int Id) : IRequest<Lesson>;

    internal sealed class GetLessonById_QueryHandler(ILessonRepository lessonRepository) : IRequestHandler<GetLessonById_Query, Lesson>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;

        public Task<Lesson> Handle(GetLessonById_Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_lessonRepository.GetById(request.Id));
        }
    }
}
