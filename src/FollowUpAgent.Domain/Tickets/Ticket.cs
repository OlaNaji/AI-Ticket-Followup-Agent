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

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public DateTimeOffset? DueDate { get; private set; }

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
}
