namespace FollowUpAgent.Application.Tickets;

public sealed record CreateTicketResult
{
    private CreateTicketResult(
        bool isSuccess,
        Guid? ticketId,
        string? errorCode,
        string? errorMessage)
    {
        IsSuccess = isSuccess;
        TicketId = ticketId;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public Guid? TicketId { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }

    public static CreateTicketResult Success(Guid ticketId)
    {
        return new CreateTicketResult(true, ticketId, null, null);
    }

    public static CreateTicketResult Failure(string errorCode, string errorMessage)
    {
        return new CreateTicketResult(false, null, errorCode, errorMessage);
    }
}
