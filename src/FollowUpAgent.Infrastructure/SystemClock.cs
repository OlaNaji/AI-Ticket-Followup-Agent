using FollowUpAgent.Application.Common;

namespace FollowUpAgent.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
