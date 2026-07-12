using FollowUpAgent.Domain.Users;
using FollowUpAgent.Infrastructure.Persistence.Repositories;
using FollowUpAgent.Infrastructure.Tests.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FollowUpAgent.Infrastructure.Tests.Persistence.Repositories;

public sealed class EfUserRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUser()
    {
        await using var database = await SqliteTestDatabase.CreateAsync();
        var user = User.Create("Ola Naji", "ola@example.com", UserRole.Agent);
        database.DbContext.Users.Add(user);
        await database.DbContext.SaveChangesAsync();
        var repository = new EfUserRepository(database.DbContext);

        var foundUser = await repository.GetByIdAsync(user.Id);

        Assert.NotNull(foundUser);
        Assert.Equal(user.Id, foundUser.Id);
        Assert.Equal("Ola Naji", foundUser.DisplayName);
        Assert.Equal("ola@example.com", foundUser.Email);
        Assert.Equal(UserRole.Agent, foundUser.Role);
        Assert.True(foundUser.IsActive);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        await using var database = await SqliteTestDatabase.CreateAsync();
        var repository = new EfUserRepository(database.DbContext);

        var foundUser = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(foundUser);
    }
}
