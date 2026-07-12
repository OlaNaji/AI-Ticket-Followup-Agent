using FollowUpAgent.Application.Tickets;
using FollowUpAgent.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace FollowUpAgent.Infrastructure.Persistence.Repositories;

public sealed class EfTicketRepository : ITicketRepository
{
    private readonly FollowUpAgentDbContext _dbContext;

    public EfTicketRepository(FollowUpAgentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        await _dbContext.Tickets.AddAsync(ticket, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
