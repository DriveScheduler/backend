namespace Domain.Enums
{
    public enum UserLessonState
    {
        Free,
        BookedByOther,
        BookedByUser,
        InWaitingList
    }

    public static class LessonStateExtension
    {
        public static string ToText(this UserLessonState state)
        {
            return state switch
            {
                UserLessonState.Free => "Libre",
                UserLessonState.BookedByOther => "Réservé par quelqu'un d'autre",
                UserLessonState.BookedByUser => "Réservé par vous",
                UserLessonState.InWaitingList => "En liste d'attente",
                _ => string.Empty,
            };
        }
    }
}
