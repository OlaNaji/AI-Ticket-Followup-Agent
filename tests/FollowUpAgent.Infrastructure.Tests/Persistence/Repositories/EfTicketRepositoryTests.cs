using FollowUpAgent.Domain.Tickets;
using FollowUpAgent.Infrastructure.Persistence.Repositories;
using FollowUpAgent.Infrastructure.Tests.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FollowUpAgent.Infrastructure.Tests.Persistence.Repositories;

public sealed class EfTicketRepositoryTests
{
    [Fact]
    public async Task AddAsync_SavesTicket()
    {
        await using var database = await SqliteTestDatabase.CreateAsync();
        var repository = new EfTicketRepository(database.DbContext);
        var createdAt = new DateTimeOffset(2026, 7, 12, 10, 0, 0, TimeSpan.Zero);
        var dueDate = createdAt.AddDays(2);
        var ticket = Ticket.Create(
            "Follow up quotation approval",
            Guid.NewGuid(),
            createdAt,
            TicketPriority.High,
            "Client is waiting for revised quotation.",
            dueDate);

        await repository.AddAsync(ticket);

        var savedTicket = await database.DbContext.Tickets.SingleAsync();
        Assert.Equal(ticket.Id, savedTicket.Id);
        Assert.Equal("Follow up quotation approval", savedTicket.Title);
        Assert.Equal(TicketStatus.New, savedTicket.Status);
        Assert.Equal(TicketPriority.High, savedTicket.Priority);
        Assert.Equal(createdAt, savedTicket.CreatedAt);
        Assert.Equal(createdAt, savedTicket.UpdatedAt);
        Assert.Equal(dueDate, savedTicket.DueDate);
        Assert.Null(savedTicket.CompletedAt);
    }
}
