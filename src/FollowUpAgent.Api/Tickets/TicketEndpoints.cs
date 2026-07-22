using FollowUpAgent.Application.Tickets;

namespace FollowUpAgent.Api.Tickets;

public static class TicketEndpoints
{
    public static IEndpointRouteBuilder MapTicketEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/tickets")
            .WithTags("Tickets");

        group.MapPost("/", CreateTicketAsync)
            .WithName("CreateTicket")
            .Produces<CreateTicketResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status403Forbidden)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> CreateTicketAsync(
        CreateTicketRequest request,
        CreateTicketUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateTicketCommand(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.CreatedByUserId);

        var result = await useCase.HandleAsync(command, cancellationToken);

        if (result.IsSuccess && result.TicketId is not null)
        {
            return Results.Created(
                $"/api/tickets/{result.TicketId}",
                new CreateTicketResponse(result.TicketId.Value));
        }

        var error = new ErrorResponse(
            result.ErrorCode ?? "unknown_error",
            result.ErrorMessage ?? "The ticket could not be created.");

        return result.ErrorCode switch
        {
            "creator_not_found" => Results.NotFound(error),
            "creator_inactive" or "creator_not_authorized" => Results.Json(
                error,
                statusCode: StatusCodes.Status403Forbidden),
            _ => Results.BadRequest(error)
        };
    }
}
