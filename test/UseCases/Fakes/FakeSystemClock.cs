using Domain.Abstractions;

namespace UseCases.Fakes
{
    internal class FakeSystemClock(DateTime now) : ISystemClock
    {
        private readonly DateTime _now = now;

        public DateTime Now => _now;
    }
}
