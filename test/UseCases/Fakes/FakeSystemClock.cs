using Application.Abstractions;

namespace UseCases.Fakes
{
    internal class FakeSystemClock(DateTime now) : ISystemClock
    {
        private DateTime _now = now;

        public DateTime Now => _now;

        public void Set(DateTime now) => _now = now;        
    }
}
