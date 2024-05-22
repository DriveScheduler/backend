using Domain.Enums;

namespace API.Outputs
{
    public sealed class LessonStateOutput
    {
        public int Value { get; }
        public string Label { get; }

        public LessonStateOutput(UserLessonState lessonState)
        {
            Value = (int)lessonState;
            Label = lessonState.ToText();
        }
    }
}
