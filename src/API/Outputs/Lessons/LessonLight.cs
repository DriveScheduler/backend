using Domain.Entities.Database;

namespace API.Outputs.Lessons
{
    public sealed class LessonLight
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; }
        public string Status { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public string Time { get; }
        public string Duration { get; }
        public string Teacher { get; }
        public string Vehicle { get; }

        public LessonLight(Lesson lesson)
        {
            Id = lesson.Id;
            Title = lesson.Name;            
            Date = lesson.Start.ToShortDateString();
            Status = "PAS ENCORE FAIT";
            StartTime = lesson.Start;
            EndTime = lesson.End;
            Time = $"{lesson.Start:HH:mm} à {lesson.End:HH:mm}";
            Duration = $"{lesson.Duration} min";

            Teacher = lesson.Teacher != null ?
                $"{lesson.Teacher.FirstName}" : "Erreur";

            Vehicle = lesson.Vehicle != null ?
                $"{lesson.Vehicle.Name}" : "Erreur";
        }
    }
}
