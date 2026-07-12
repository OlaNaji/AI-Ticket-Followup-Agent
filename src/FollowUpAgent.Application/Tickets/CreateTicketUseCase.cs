using FollowUpAgent.Application.Common;
using FollowUpAgent.Application.Users;
using FollowUpAgent.Domain.Tickets;
using FollowUpAgent.Domain.Users;

namespace FollowUpAgent.Application.Tickets;

public sealed class CreateTicketUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IClock _clock;

    public CreateTicketUseCase(
        IUserRepository userRepository,
        ITicketRepository ticketRepository,
        IClock clock)
    {
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
        _clock = clock;
    }

    public async Task<CreateTicketResult> HandleAsync(
        CreateTicketCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CreatedByUserId == Guid.Empty)
        {
            return CreateTicketResult.Failure(
                "invalid_creator_id",
                "A valid ticket creator is required.");
        }

        var creator = await _userRepository.GetByIdAsync(command.CreatedByUserId, cancellationToken);

        if (creator is null)
        {
            return CreateTicketResult.Failure(
                "creator_not_found",
                "The ticket creator does not exist.");
        }

        if (!creator.IsActive)
        {
            return CreateTicketResult.Failure(
                "creator_inactive",
                "The ticket creator is inactive.");
        }

        if (!CanCreateTicket(creator.Role))
        {
            return CreateTicketResult.Failure(
                "creator_not_authorized",
                "The ticket creator is not allowed to create tickets.");
        }

        Ticket ticket;

        try
        {
            ticket = Ticket.Create(
                command.Title,
                command.CreatedByUserId,
                _clock.UtcNow,
                command.Priority,
                command.Description,
                command.DueDate);
        }
        catch (ArgumentOutOfRangeException exception)
        {
            return CreateTicketResult.Failure(
                ToErrorCode(exception.ParamName),
                exception.Message);
        }
        catch (ArgumentException exception)
        {
            return CreateTicketResult.Failure(
                ToErrorCode(exception.ParamName),
                exception.Message);
        }

        await _ticketRepository.AddAsync(ticket, cancellationToken);

        return CreateTicketResult.Success(ticket.Id);
    }

    private static bool CanCreateTicket(UserRole role)
    {
        return role is UserRole.Admin or UserRole.Manager or UserRole.Agent;
    }

    private static string ToErrorCode(string? parameterName)
    {
        return parameterName switch
        {
            "title" => "invalid_title",
            "priority" => "invalid_priority",
            "dueDate" => "invalid_due_date",
            "createdByUserId" => "invalid_creator_id",
            _ => "invalid_request"
        };
    }
}
