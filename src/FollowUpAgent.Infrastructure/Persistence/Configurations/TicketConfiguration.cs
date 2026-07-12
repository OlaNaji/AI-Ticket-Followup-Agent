using FollowUpAgent.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FollowUpAgent.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(ticket => ticket.Id);

        builder.Property(ticket => ticket.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ticket => ticket.Description)
            .HasMaxLength(4000);

        builder.Property(ticket => ticket.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(ticket => ticket.Priority)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(ticket => ticket.CreatedByUserId)
            .IsRequired();

        builder.Property(ticket => ticket.AssignedToUserId);

        builder.Property(ticket => ticket.CreatedAt)
            .IsRequired();

        builder.Property(ticket => ticket.UpdatedAt)
            .IsRequired();

        builder.Property(ticket => ticket.DueDate);

        builder.Property(ticket => ticket.CompletedAt);

        builder.HasIndex(ticket => ticket.Status);

        builder.HasIndex(ticket => ticket.Priority);

        builder.HasIndex(ticket => ticket.CreatedByUserId);

        builder.HasIndex(ticket => ticket.AssignedToUserId);
    }
}
