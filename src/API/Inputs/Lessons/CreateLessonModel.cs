﻿using Domain.Enums;

namespace API.Inputs.Lessons
{
    public sealed class CreateLessonModel
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public Guid TeacherId { get; set; }
        public LicenceType Type { get; set; }
        public int VehicleId { get; set; }
        public int MaxStudent { get; set; }
    }
}
