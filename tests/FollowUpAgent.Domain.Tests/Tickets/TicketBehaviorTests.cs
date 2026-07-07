using FollowUpAgent.Domain.Tickets;

namespace FollowUpAgent.Domain.Tests.Tickets;

public sealed class TicketBehaviorTests
{
    [Fact]
    public void AssignTo_WithValidAssignee_AssignsTicketAndUpdatesTimestamp()
    {
        var ticket = CreateTicket();
        var assigneeId = Guid.NewGuid();
        var changedAt = ticket.UpdatedAt.AddHours(1);

        ticket.AssignTo(assigneeId, changedAt);

        Assert.Equal(assigneeId, ticket.AssignedToUserId);
        Assert.Equal(changedAt, ticket.UpdatedAt);
    }

    [Fact]
    public void AssignTo_WithEmptyAssigneeId_ThrowsArgumentException()
    {
        var ticket = CreateTicket();

        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.AssignTo(Guid.Empty, ticket.UpdatedAt.AddHours(1)));

        Assert.Equal("assignedToUserId", exception.ParamName);
    }

    [Fact]
    public void AssignTo_WithSameAssignee_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();
        var assigneeId = Guid.NewGuid();
        ticket.AssignTo(assigneeId, ticket.UpdatedAt.AddHours(1));

        Assert.Throws<InvalidOperationException>(() =>
            ticket.AssignTo(assigneeId, ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void AssignTo_WhenTicketIsDone_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();
        ticket.Complete(ticket.UpdatedAt.AddHours(1));

        Assert.Throws<InvalidOperationException>(() =>
            ticket.AssignTo(Guid.NewGuid(), ticket.UpdatedAt.AddHours(1)));
    }

    [Theory]
    [InlineData(TicketStatus.New, TicketStatus.InProgress)]
    [InlineData(TicketStatus.New, TicketStatus.Waiting)]
    [InlineData(TicketStatus.New, TicketStatus.Blocked)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Waiting)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Blocked)]
    [InlineData(TicketStatus.Waiting, TicketStatus.InProgress)]
    [InlineData(TicketStatus.Waiting, TicketStatus.Blocked)]
    [InlineData(TicketStatus.Blocked, TicketStatus.InProgress)]
    [InlineData(TicketStatus.Blocked, TicketStatus.Waiting)]
    public void ChangeStatus_WithAllowedTransition_ChangesStatusAndUpdatesTimestamp(
        TicketStatus currentStatus,
        TicketStatus newStatus)
    {
        var ticket = CreateTicketAtStatus(currentStatus);
        var changedAt = ticket.UpdatedAt.AddHours(1);

        ticket.ChangeStatus(newStatus, changedAt);

        Assert.Equal(newStatus, ticket.Status);
        Assert.Equal(changedAt, ticket.UpdatedAt);
    }

    [Fact]
    public void ChangeStatus_ToSameStatus_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();

        Assert.Throws<InvalidOperationException>(() =>
            ticket.ChangeStatus(TicketStatus.New, ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void ChangeStatus_ToDone_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();

        Assert.Throws<InvalidOperationException>(() =>
            ticket.ChangeStatus(TicketStatus.Done, ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void ChangeStatus_FromDone_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();
        ticket.Complete(ticket.UpdatedAt.AddHours(1));

        Assert.Throws<InvalidOperationException>(() =>
            ticket.ChangeStatus(TicketStatus.InProgress, ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void ChangeStatus_WithInvalidTransition_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicketAtStatus(TicketStatus.Blocked);

        Assert.Throws<InvalidOperationException>(() =>
            ticket.ChangeStatus(TicketStatus.New, ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void ChangeStatus_WithInvalidStatus_ThrowsArgumentOutOfRangeException()
    {
        var ticket = CreateTicket();
        var invalidStatus = (TicketStatus)999;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ticket.ChangeStatus(invalidStatus, ticket.UpdatedAt.AddHours(1)));

        Assert.Equal("newStatus", exception.ParamName);
    }

    [Theory]
    [InlineData(TicketStatus.New)]
    [InlineData(TicketStatus.InProgress)]
    [InlineData(TicketStatus.Waiting)]
    [InlineData(TicketStatus.Blocked)]
    public void Complete_FromActiveStatus_MarksTicketDoneAndSetsCompletionTimestamp(TicketStatus status)
    {
        var ticket = CreateTicketAtStatus(status);
        var completedAt = ticket.UpdatedAt.AddHours(1);

        ticket.Complete(completedAt);

        Assert.Equal(TicketStatus.Done, ticket.Status);
        Assert.Equal(completedAt, ticket.CompletedAt);
        Assert.Equal(completedAt, ticket.UpdatedAt);
    }

    [Fact]
    public void Complete_WhenAlreadyDone_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();
        ticket.Complete(ticket.UpdatedAt.AddHours(1));

        Assert.Throws<InvalidOperationException>(() =>
            ticket.Complete(ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void Reopen_WhenDone_SetsStatusToInProgressAndClearsCompletedAt()
    {
        var ticket = CreateTicket();
        ticket.Complete(ticket.UpdatedAt.AddHours(1));
        var reopenedAt = ticket.UpdatedAt.AddHours(1);

        ticket.Reopen(reopenedAt);

        Assert.Equal(TicketStatus.InProgress, ticket.Status);
        Assert.Null(ticket.CompletedAt);
        Assert.Equal(reopenedAt, ticket.UpdatedAt);
    }

    [Fact]
    public void Reopen_WhenNotDone_ThrowsInvalidOperationException()
    {
        var ticket = CreateTicket();

        Assert.Throws<InvalidOperationException>(() =>
            ticket.Reopen(ticket.UpdatedAt.AddHours(1)));
    }

    [Fact]
    public void AssignTo_WithChangedAtEarlierThanUpdatedAt_ThrowsArgumentException()
    {
        var ticket = CreateTicket();
        var earlierThanUpdatedAt = ticket.UpdatedAt.AddTicks(-1);

        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.AssignTo(Guid.NewGuid(), earlierThanUpdatedAt));

        Assert.Equal("changedAt", exception.ParamName);
    }

    [Fact]
    public void ChangeStatus_WithChangedAtEarlierThanUpdatedAt_ThrowsArgumentException()
    {
        var ticket = CreateTicket();
        var earlierThanUpdatedAt = ticket.UpdatedAt.AddTicks(-1);

        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.ChangeStatus(TicketStatus.InProgress, earlierThanUpdatedAt));

        Assert.Equal("changedAt", exception.ParamName);
    }

    [Fact]
    public void Complete_WithCompletedAtEarlierThanUpdatedAt_ThrowsArgumentException()
    {
        var ticket = CreateTicket();
        var earlierThanUpdatedAt = ticket.UpdatedAt.AddTicks(-1);

        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.Complete(earlierThanUpdatedAt));

        Assert.Equal("changedAt", exception.ParamName);
    }

    private static Ticket CreateTicket()
    {
        return Ticket.Create(
            "Follow up project status",
            Guid.NewGuid(),
            new DateTimeOffset(2026, 7, 7, 10, 0, 0, TimeSpan.Zero),
            TicketPriority.Medium);
    }

    private static Ticket CreateTicketAtStatus(TicketStatus status)
    {
        var ticket = CreateTicket();

        if (status == TicketStatus.New)
        {
            return ticket;
        }

        if (status == TicketStatus.Done)
        {
            ticket.Complete(ticket.UpdatedAt.AddHours(1));
            return ticket;
        }

        ticket.ChangeStatus(status, ticket.UpdatedAt.AddHours(1));
        return ticket;
    }
}
