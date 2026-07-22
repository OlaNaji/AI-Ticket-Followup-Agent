using FollowUpAgent.Domain.Tickets;

namespace FollowUpAgent.Api.Tickets;

public sealed record CreateTicketRequest(
    string Title,
    string? Description,
    TicketPriority Priority,
    DateTimeOffset? DueDate,
    Guid CreatedByUserId);
