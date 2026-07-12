using FollowUpAgent.Application.Users;
using FollowUpAgent.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FollowUpAgent.Infrastructure.Persistence.Repositories;

public sealed class EfUserRepository : IUserRepository
{
    private readonly FollowUpAgentDbContext _dbContext;

    public EfUserRepository(FollowUpAgentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
}
