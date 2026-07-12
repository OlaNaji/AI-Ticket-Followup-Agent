using FollowUpAgent.Domain.Tickets;

namespace FollowUpAgent.Application.Tickets;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
}
