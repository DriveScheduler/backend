namespace Domain.Abstractions
{
    public interface ISystemClock
    {
        public DateTime Now { get; }
    }
}
