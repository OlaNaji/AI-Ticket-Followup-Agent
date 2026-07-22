using FollowUpAgent.Api.Tickets;
using FollowUpAgent.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { name = "AI Ticket & Project Follow-up Agent" }));

app.MapTicketEndpoints();

app.Run();

public partial class Program;
