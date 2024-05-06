using Domain.Enums;

namespace API.Inputs.Lessons
{
    public class UpdateLessonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public Guid TeacherId { get; set; }
        public LicenceType Type { get; set; }
        public string VehicleId { get; set; }
        public int MaxStudent { get; set; }
    }
}
