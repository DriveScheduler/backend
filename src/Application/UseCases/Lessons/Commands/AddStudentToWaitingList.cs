using Application.Abstractions;

using Domain.Models;
using Domain.Exceptions.Lessons;
using Domain.Repositories;

using MediatR;

namespace Application.UseCases.Lessons.Commands
{
    public sealed record AddStudentToWaitingList_Command(int LessonId, Guid UserId) : IRequest;

    internal sealed class AddStudentToWaitingList_CommandHandler(
        ILessonRepository lessonRepository, 
        IUserRepository userRepository,
        ISystemClock systemClock) : IRequestHandler<AddStudentToWaitingList_Command>
    {
        private readonly ILessonRepository _lessonRepository = lessonRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISystemClock _systemClock = systemClock;

        public Task Handle(AddStudentToWaitingList_Command request, CancellationToken cancellationToken)
        {
            User student = _userRepository.GetUserById(request.UserId);
            //User? student = _database.Users.Find(request.UserId);
            //if (student is null)
            //    throw new UserNotFoundException();

            Lesson lesson = _lessonRepository.GetById(request.LessonId);
            //Lesson? lesson = _database.Lessons
            //    .Include(Lesson => Lesson.Student)
            //    .Include(Lesson => Lesson.WaitingList)
            //    .FirstOrDefault(l => l.Id == request.LessonId);
            //if (lesson is null)
            //    throw new LessonNotFoundException();
                       
            if (lesson.Start >= _systemClock.Now)
                throw new LessonValidationException("Le cours est déjà passé");
            
            lesson.AddStudentToWaitingList(student);
            //lesson.WaitingList.Add(student);            
            //new LessonValidator(_systemClock)
            //    .AddStudentToWaitingListRules()
            //    .ThrowIfInvalid(lesson);            

            _lessonRepository.Update(lesson);
            return Task.CompletedTask;
            //if (await _database.SaveChangesAsync() != 1)
            //    throw new LessonSaveException();
        }
    }
}
