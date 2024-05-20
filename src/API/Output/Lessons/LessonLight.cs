﻿using Domain.Entities.Database;
using System.Globalization;

namespace API.Output.Lessons
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
            Date = $"{lesson.Start.ToString("dddd", new CultureInfo("fr-FR"))} - {lesson.Start:dd/MM/yyyy}";
            Status = "PAS ENCORE FAIT";
            StartTime = lesson.Start;
            EndTime = lesson.End;
            Time = $"{lesson.Start:HHhmm} à {lesson.End:HHhmm}";
            Duration = $"{lesson.Duration} min";

            Teacher = lesson.Teacher != null ?
                $"{lesson.Teacher.FirstName}" : "Erreur";

            Vehicle = lesson.Vehicle != null ?
                $"{lesson.Vehicle.Name}" : "Erreur";
        }
    }
}
