namespace FollowUpAgent.Domain.Tickets;

public sealed class Ticket
{
    private Ticket(
        Guid id,
        string title,
        string? description,
        Guid createdByUserId,
        DateTimeOffset createdAt,
        TicketPriority priority,
        DateTimeOffset? dueDate)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        Status = TicketStatus.New;
        Priority = priority;
        DueDate = dueDate;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    public TicketStatus Status { get; private set; }

    public TicketPriority Priority { get; private set; }

    public Guid CreatedByUserId { get; }

    public Guid? AssignedToUserId { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public DateTimeOffset? DueDate { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public void AssignTo(Guid assignedToUserId, DateTimeOffset changedAt)
    {
        if (assignedToUserId == Guid.Empty)
        {
            throw new ArgumentException("A valid assignee is required.", nameof(assignedToUserId));
        }

        EnsureTicketIsNotDone("Cannot assign a completed ticket.");
        EnsureChangeIsNotInThePast(changedAt);

        if (AssignedToUserId == assignedToUserId)
        {
            throw new InvalidOperationException("Ticket is already assigned to this user.");
        }

        AssignedToUserId = assignedToUserId;
        UpdatedAt = changedAt;
    }

    public void ChangeStatus(TicketStatus newStatus, DateTimeOffset changedAt)
    {
        if (!Enum.IsDefined(newStatus))
        {
            throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, "Ticket status is invalid.");
        }

        if (newStatus == TicketStatus.Done)
        {
            throw new InvalidOperationException("Use Complete() to mark a ticket as done.");
        }

        EnsureTicketIsNotDone("Use Reopen() before changing the status of a completed ticket.");
        EnsureChangeIsNotInThePast(changedAt);

        if (Status == newStatus)
        {
            throw new InvalidOperationException("Ticket is already in this status.");
        }

        if (!IsAllowedTransition(Status, newStatus))
        {
            throw new InvalidOperationException($"Cannot change ticket status from {Status} to {newStatus}.");
        }

        Status = newStatus;
        UpdatedAt = changedAt;
    }

    public void Complete(DateTimeOffset completedAt)
    {
        EnsureTicketIsNotDone("Ticket is already completed.");
        EnsureChangeIsNotInThePast(completedAt);

        Status = TicketStatus.Done;
        CompletedAt = completedAt;
        UpdatedAt = completedAt;
    }

    public void Reopen(DateTimeOffset reopenedAt)
    {
        if (Status != TicketStatus.Done)
        {
            throw new InvalidOperationException("Only completed tickets can be reopened.");
        }

        EnsureChangeIsNotInThePast(reopenedAt);

        Status = TicketStatus.InProgress;
        CompletedAt = null;
        UpdatedAt = reopenedAt;
    }

    public static Ticket Create(
        string title,
        Guid createdByUserId,
        DateTimeOffset createdAt,
        TicketPriority priority,
        string? description = null,
        DateTimeOffset? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Ticket title is required.", nameof(title));
        }

        if (createdByUserId == Guid.Empty)
        {
            throw new ArgumentException("A valid creator is required.", nameof(createdByUserId));
        }

        if (!Enum.IsDefined(priority))
        {
            throw new ArgumentOutOfRangeException(nameof(priority), priority, "Ticket priority is invalid.");
        }

        if (dueDate is not null && dueDate.Value < createdAt)
        {
            throw new ArgumentException("Due date cannot be earlier than the creation date.", nameof(dueDate));
        }

        return new Ticket(
            Guid.NewGuid(),
            title.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            createdByUserId,
            createdAt,
            priority,
            dueDate);
    }

    private static bool IsAllowedTransition(TicketStatus currentStatus, TicketStatus newStatus)
    {
        return currentStatus switch
        {
            TicketStatus.New => newStatus is TicketStatus.InProgress or TicketStatus.Waiting or TicketStatus.Blocked,
            TicketStatus.InProgress => newStatus is TicketStatus.Waiting or TicketStatus.Blocked,
            TicketStatus.Waiting => newStatus is TicketStatus.InProgress or TicketStatus.Blocked,
            TicketStatus.Blocked => newStatus is TicketStatus.InProgress or TicketStatus.Waiting,
            _ => false
        };
    }

    private void EnsureTicketIsNotDone(string message)
    {
        if (Status == TicketStatus.Done)
        {
            throw new InvalidOperationException(message);
        }
    }

    private void EnsureChangeIsNotInThePast(DateTimeOffset changedAt)
    {
        if (changedAt < UpdatedAt)
        {
            throw new ArgumentException("Change date cannot be earlier than the last update date.", nameof(changedAt));
        }
    }
}
