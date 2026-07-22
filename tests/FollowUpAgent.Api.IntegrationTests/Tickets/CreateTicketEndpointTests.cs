using System.Net;
using System.Net.Http.Json;
using FollowUpAgent.Api.Tickets;
using FollowUpAgent.Domain.Tickets;
using FollowUpAgent.Domain.Users;
using FollowUpAgent.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FollowUpAgent.Api.IntegrationTests.Tickets;

public sealed class CreateTicketEndpointTests
{
    [Fact]
    public async Task PostTickets_WithAuthorizedCreator_ReturnsCreatedAndSavesTicket()
    {
        await using var factory = await ApiTestApplicationFactory.CreateAsync();
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        await factory.SeedUserAsync(creator);
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest(
            "Follow up invoice approval",
            "Client needs confirmation.",
            TicketPriority.High,
            DateTimeOffset.UtcNow.AddDays(2),
            creator.Id));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CreateTicketResponse>();
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.TicketId);

        var savedTicket = await factory.FindTicketAsync(body.TicketId);
        Assert.NotNull(savedTicket);
        Assert.Equal("Follow up invoice approval", savedTicket.Title);
        Assert.Equal(creator.Id, savedTicket.CreatedByUserId);
        Assert.Equal(TicketPriority.High, savedTicket.Priority);
    }

    [Fact]
    public async Task PostTickets_WhenCreatorDoesNotExist_ReturnsNotFound()
    {
        await using var factory = await ApiTestApplicationFactory.CreateAsync();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest(
            "Follow up invoice approval",
            null,
            TicketPriority.Medium,
            null,
            Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Equal("creator_not_found", error.Code);
    }

    [Fact]
    public async Task PostTickets_WhenCreatorIsViewer_ReturnsForbidden()
    {
        await using var factory = await ApiTestApplicationFactory.CreateAsync();
        var creator = User.Create("Read Only", "viewer@example.com", UserRole.Viewer);
        await factory.SeedUserAsync(creator);
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest(
            "Follow up invoice approval",
            null,
            TicketPriority.Medium,
            null,
            creator.Id));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Equal("creator_not_authorized", error.Code);
    }

    [Fact]
    public async Task PostTickets_WithInvalidTitle_ReturnsBadRequest()
    {
        await using var factory = await ApiTestApplicationFactory.CreateAsync();
        var creator = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        await factory.SeedUserAsync(creator);
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/tickets", new CreateTicketRequest(
            " ",
            null,
            TicketPriority.Medium,
            null,
            creator.Id));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Equal("invalid_title", error.Code);
    }

    private sealed class ApiTestApplicationFactory : WebApplicationFactory<Program>, IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private ApiTestApplicationFactory(SqliteConnection connection)
        {
            _connection = connection;
        }

        public static async Task<ApiTestApplicationFactory> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var factory = new ApiTestApplicationFactory(connection);

            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FollowUpAgentDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            return factory;
        }

        public async Task SeedUserAsync(User user)
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FollowUpAgentDbContext>();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Ticket?> FindTicketAsync(Guid ticketId)
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FollowUpAgentDbContext>();
            return await dbContext.Tickets.SingleOrDefaultAsync(ticket => ticket.Id == ticketId);
        }

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<FollowUpAgentDbContext>>();
                services.AddDbContext<FollowUpAgentDbContext>(options =>
                    options.UseSqlite(_connection));
            });
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
