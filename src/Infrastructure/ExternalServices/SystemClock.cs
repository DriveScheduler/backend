using Application.Abstractions;

namespace Infrastructure.ExternalServices
{
    internal class SystemClock : ISystemClock
    {
        public DateTime Now => DateTime.Now;
    }
}
