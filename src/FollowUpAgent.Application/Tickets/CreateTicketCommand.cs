using FollowUpAgent.Domain.Tickets;

namespace FollowUpAgent.Application.Tickets;

public sealed record CreateTicketCommand(
    string Title,
    string? Description,
    TicketPriority Priority,
    DateTimeOffset? DueDate,
    Guid CreatedByUserId);
