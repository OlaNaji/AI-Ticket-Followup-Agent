using FollowUpAgent.Domain.Users;

namespace FollowUpAgent.Application.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
