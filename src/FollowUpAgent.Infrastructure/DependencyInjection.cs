using FollowUpAgent.Application.Common;
using FollowUpAgent.Application.Tickets;
using FollowUpAgent.Application.Users;
using FollowUpAgent.Infrastructure.Persistence;
using FollowUpAgent.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FollowUpAgent.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FollowUpAgent")
            ?? "Data Source=followup-agent-dev.db";

        services.AddDbContext<FollowUpAgentDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<ITicketRepository, EfTicketRepository>();
        services.AddScoped<CreateTicketUseCase>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}
