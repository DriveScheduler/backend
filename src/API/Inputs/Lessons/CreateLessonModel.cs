﻿using Domain.Enums;

namespace API.Inputs.Lessons
{
    public sealed class CreateLessonModel
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public int Duration { get; set; }
    }
}
