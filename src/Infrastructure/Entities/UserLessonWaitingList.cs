namespace Infrastructure.Entities
{
    internal class UserLessonWaitingList
    {
        public int LessonId { get; set; }
        public Guid UserId { get; set; }
        public LessonDataEntity Lesson { get; set; }
        public UserDataEntity User { get; set;}
    }
}
