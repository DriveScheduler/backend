namespace Domain.Exceptions.Lessons
{
    public sealed class LessonSaveException : Exception
    {
        public LessonSaveException() : base("Une erreur s'est produite pendant la sauvegarde du cours")
        {
        }
    }
}
