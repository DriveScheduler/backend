namespace Application.Abstractions
{
    public interface ISystemClock
    {
        public DateTime Now { get; }
    }
}
