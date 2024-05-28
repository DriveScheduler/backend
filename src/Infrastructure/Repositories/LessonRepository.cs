using Application.Abstractions;

using Domain.Enums;
using Domain.Exceptions.Lessons;
using Domain.Models;
using Domain.Repositories;

using Infrastructure.Entities;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal sealed class LessonRepository(DatabaseContext database) : ILessonRepository
    {
        private readonly DatabaseContext _database = database;

        public Task<List<Lesson>> GetAllAsync()
        {
            return IncludeAllSubEntities()
                .Select(lesson => lesson.ToDomainModel())
                .ToListAsync();
        }

        public Task<List<Lesson>> GetAllStudentLesson(Guid userId)
        {
            return IncludeAllSubEntities()
                .Where(lesson => lesson.Student != null && lesson.Student.Id == userId)
                .Select(lesson => lesson.ToDomainModel())
                .ToListAsync();
        }

        public async Task<Lesson> GetByIdAsync(int id)
        {
            Lesson_Database? lesson = await _database.Lessons.FirstOrDefaultAsync(l => l.Id == id);
            if (lesson is null)
                throw new LessonNotFoundException();
            return lesson.ToDomainModel();
        }

        public async Task<List<Lesson>> GetLessonsForUserAsync(User user, DateTime start, DateTime end, bool onlyEmptyLesson = false)
        {
            IQueryable<Lesson_Database> query = IncludeAllSubEntities();
            if (user.Type == UserType.Student)
                query = query.Where(lesson => lesson.Type == user.LicenceType);
            else if (user.Type == UserType.Teacher)
                query = query.Where(lesson => lesson.Teacher.Id == user.Id);


            if (onlyEmptyLesson)
                query = query.Where(lesson => lesson.Student == null);


            DateTime calculatedEndDate = end.AddDays(1).Date;
            return await query
                .Where(lesson => lesson.Start >= start.Date && lesson.Start.AddMinutes(lesson.Duration) <= calculatedEndDate)
                .Select(l => l.ToDomainModel())
                .ToListAsync();
        }

        public Task<List<Lesson>> GetPassedLesson(Guid userId, DateTime now)
        {
            return IncludeAllSubEntities()
                .Where(lesson => lesson.Student != null && lesson.Student.Id == userId && lesson.Start > now)
                .OrderBy(lesson => lesson.Start)
                .Select(lesson => lesson.ToDomainModel())
                .ToListAsync();
        }

        public Task<List<Lesson>> GetUserHistory(Guid userId, DateTime now)
        {
            return IncludeAllSubEntities()
                .Where(lesson => lesson.Student != null && lesson.Student.Id == userId && (lesson.Start.AddMinutes(lesson.Duration)) <= now)
                .OrderByDescending(lesson => lesson.Start)
                .Select(lesson => lesson.ToDomainModel())
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetUserPlanning(User user, DateTime start, DateTime end)
        {
            DateTime calculatedEndDate = end.Date.AddDays(1).Date;

            IQueryable<Lesson_Database> query = IncludeAllSubEntities();
            if (user.Type == UserType.Student)
                query = query
                    .Where(lesson => lesson.Student != null && lesson.Student.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);
            else if (user.Type == UserType.Teacher)
                query = query
                    .Where(lesson => lesson.Teacher.Id == user.Id && lesson.Start >= start && lesson.Start <= calculatedEndDate);

            return await query
                .Select(lesson => lesson.ToDomainModel())
                .ToListAsync();

        }

        public int Insert(Lesson lesson)
        {
            Lesson_Database dbLesson = new(lesson);
            try
            {
                _database.Lessons.Add(dbLesson);
                _database.SaveChanges();
                return dbLesson.Id;
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        public List<int> Insert(List<Lesson> lessons)
        {
            List<Lesson_Database> dbLessons = lessons.Select(l => new Lesson_Database(l)).ToList();
            try
            {
                _database.Lessons.AddRange(dbLessons);
                _database.SaveChanges();
                return dbLessons.Select(l => l.Id).ToList();
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        public async Task<int> InsertAsync(Lesson lesson)
        {
            Lesson_Database dbLesson = new(lesson);
            try
            {
                _database.Lessons.Add(dbLesson);
                await _database.SaveChangesAsync();
                return dbLesson.Id;
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            Lesson_Database? dbLesson = await _database.Lessons.FindAsync(lesson.Id);
            if (dbLesson is null) throw new LessonNotFoundException();
            dbLesson.FromDomainModel(lesson);
            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new LessonSaveException();
            }
        }

        private IQueryable<Lesson_Database> IncludeAllSubEntities()
        {
            return _database.Lessons
                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Student)
                .Include(lesson => lesson.Vehicle)
                .Include(lesson => lesson.WaitingList);
        }
    }
}
