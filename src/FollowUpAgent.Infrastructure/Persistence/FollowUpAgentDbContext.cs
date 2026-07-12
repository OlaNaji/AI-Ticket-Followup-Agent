using FollowUpAgent.Domain.Tickets;
using FollowUpAgent.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FollowUpAgent.Infrastructure.Persistence;

public sealed class FollowUpAgentDbContext : DbContext
{
    public FollowUpAgentDbContext(DbContextOptions<FollowUpAgentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FollowUpAgentDbContext).Assembly);
    }
}
