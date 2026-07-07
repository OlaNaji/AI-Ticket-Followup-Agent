using FollowUpAgent.Domain.Tickets;

namespace FollowUpAgent.Domain.Tests.Tickets;

public sealed class TicketCreationTests
{
    [Fact]
    public void Create_WithValidRequiredValues_CreatesNewTicket()
    {
        var creatorId = Guid.NewGuid();
        var createdAt = new DateTimeOffset(2026, 6, 28, 10, 0, 0, TimeSpan.Zero);

        var ticket = Ticket.Create(
            "Follow up invoice approval",
            creatorId,
            createdAt,
            TicketPriority.Medium);

        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal("Follow up invoice approval", ticket.Title);
        Assert.Equal(creatorId, ticket.CreatedByUserId);
        Assert.Equal(createdAt, ticket.CreatedAt);
        Assert.Equal(createdAt, ticket.UpdatedAt);
        Assert.Equal(TicketStatus.New, ticket.Status);
        Assert.Equal(TicketPriority.Medium, ticket.Priority);
        Assert.Null(ticket.AssignedToUserId);
        Assert.Null(ticket.DueDate);
        Assert.Null(ticket.CompletedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithEmptyTitle_ThrowsArgumentException(string title)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Ticket.Create(title, Guid.NewGuid(), DateTimeOffset.UtcNow, TicketPriority.Medium));

        Assert.Equal("title", exception.ParamName);
    }

    [Fact]
    public void Create_TrimsTitle()
    {
        var ticket = Ticket.Create(
            "  Prepare quotation follow-up  ",
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            TicketPriority.Medium);

        Assert.Equal("Prepare quotation follow-up", ticket.Title);
    }

    [Fact]
    public void Create_WithEmptyCreatorId_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Ticket.Create("Follow up client visit", Guid.Empty, DateTimeOffset.UtcNow, TicketPriority.Medium));

        Assert.Equal("createdByUserId", exception.ParamName);
    }

    [Fact]
    public void Create_WithInvalidPriority_ThrowsArgumentOutOfRangeException()
    {
        var invalidPriority = (TicketPriority)999;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            Ticket.Create("Follow up blocked task", Guid.NewGuid(), DateTimeOffset.UtcNow, invalidPriority));

        Assert.Equal("priority", exception.ParamName);
    }

    [Fact]
    public void Create_WithDueDateEarlierThanCreationDate_ThrowsArgumentException()
    {
        var createdAt = new DateTimeOffset(2026, 6, 28, 10, 0, 0, TimeSpan.Zero);
        var dueDate = createdAt.AddDays(-1);

        var exception = Assert.Throws<ArgumentException>(() =>
            Ticket.Create("Follow up overdue invoice", Guid.NewGuid(), createdAt, TicketPriority.Medium, dueDate: dueDate));

        Assert.Equal("dueDate", exception.ParamName);
    }
}
