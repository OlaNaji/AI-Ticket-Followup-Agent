using FollowUpAgent.Application.Common;
using FollowUpAgent.Application.Tickets;
using FollowUpAgent.Application.Users;
using FollowUpAgent.Domain.Tickets;
using FollowUpAgent.Domain.Users;

namespace FollowUpAgent.Application.Tests.Tickets;

public sealed class CreateTicketUseCaseTests
{
    private static readonly DateTimeOffset Now = new(2026, 7, 7, 10, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Manager)]
    [InlineData(UserRole.Agent)]
    public async Task HandleAsync_WithAuthorizedActiveCreator_CreatesAndSavesTicket(UserRole role)
    {
        var creator = User.Create("Ola Naji", "ola@example.com", role);
        var users = new FakeUserRepository(creator);
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(users, tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            "Check blocker details with client",
            TicketPriority.High,
            Now.AddDays(2),
            creator.Id));

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.TicketId);
        var savedTicket = Assert.Single(tickets.SavedTickets);
        Assert.Equal(result.TicketId, savedTicket.Id);
        Assert.Equal("Follow up project status", savedTicket.Title);
        Assert.Equal(creator.Id, savedTicket.CreatedByUserId);
        Assert.Equal(TicketPriority.High, savedTicket.Priority);
        Assert.Equal(Now, savedTicket.CreatedAt);
        Assert.Equal(TicketStatus.New, savedTicket.Status);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyCreatorId_ReturnsInvalidCreatorIdFailure()
    {
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            null,
            TicketPriority.Medium,
            null,
            Guid.Empty));

        Assert.False(result.IsSuccess);
        Assert.Equal("invalid_creator_id", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    [Fact]
    public async Task HandleAsync_WhenCreatorDoesNotExist_ReturnsCreatorNotFoundFailure()
    {
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            null,
            TicketPriority.Medium,
            null,
            Guid.NewGuid()));

        Assert.False(result.IsSuccess);
        Assert.Equal("creator_not_found", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    [Fact]
    public async Task HandleAsync_WhenCreatorIsInactive_ReturnsCreatorInactiveFailure()
    {
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        creator.Deactivate();
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(creator), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            null,
            TicketPriority.Medium,
            null,
            creator.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal("creator_inactive", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    [Fact]
    public async Task HandleAsync_WhenCreatorIsViewer_ReturnsCreatorNotAuthorizedFailure()
    {
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Viewer);
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(creator), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            null,
            TicketPriority.Medium,
            null,
            creator.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal("creator_not_authorized", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidTitle_ReturnsInvalidTitleFailure()
    {
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(creator), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            " ",
            null,
            TicketPriority.Medium,
            null,
            creator.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal("invalid_title", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidPriority_ReturnsInvalidPriorityFailure()
    {
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(creator), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            null,
            (TicketPriority)999,
            null,
            creator.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal("invalid_priority", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    [Fact]
    public async Task HandleAsync_WithDueDateBeforeNow_ReturnsInvalidDueDateFailure()
    {
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        var tickets = new FakeTicketRepository();
        var useCase = CreateUseCase(new FakeUserRepository(creator), tickets);

        var result = await useCase.HandleAsync(new CreateTicketCommand(
            "Follow up project status",
            null,
            TicketPriority.Medium,
            Now.AddDays(-1),
            creator.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal("invalid_due_date", result.ErrorCode);
        Assert.Empty(tickets.SavedTickets);
    }

    private static CreateTicketUseCase CreateUseCase(
        IUserRepository userRepository,
        ITicketRepository ticketRepository)
    {
        return new CreateTicketUseCase(
            userRepository,
            ticketRepository,
            new FakeClock(Now));
    }

    private sealed class FakeClock : IClock
    {
        public FakeClock(DateTimeOffset utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTimeOffset UtcNow { get; }
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly IReadOnlyDictionary<Guid, User> _usersById;

        public FakeUserRepository(params User[] users)
        {
            _usersById = users.ToDictionary(user => user.Id);
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _usersById.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }
    }

    private sealed class FakeTicketRepository : ITicketRepository
    {
        public List<Ticket> SavedTickets { get; } = [];

        public Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
        {
            SavedTickets.Add(ticket);
            return Task.CompletedTask;
        }
    }
}
