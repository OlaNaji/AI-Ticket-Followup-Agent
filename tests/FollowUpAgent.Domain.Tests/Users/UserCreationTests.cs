using FollowUpAgent.Domain.Users;

namespace FollowUpAgent.Domain.Tests.Users;

public sealed class UserCreationTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveUser()
    {
        var user = User.Create(
            "Ola Naji",
            "ola@example.com",
            UserRole.Manager);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("Ola Naji", user.DisplayName);
        Assert.Equal("ola@example.com", user.Email);
        Assert.Equal(UserRole.Manager, user.Role);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Create_TrimsDisplayNameAndEmail()
    {
        var user = User.Create(
            "  Ola Naji  ",
            "  ola@example.com  ",
            UserRole.Agent);

        Assert.Equal("Ola Naji", user.DisplayName);
        Assert.Equal("ola@example.com", user.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithEmptyDisplayName_ThrowsArgumentException(string displayName)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            User.Create(displayName, "ola@example.com", UserRole.Agent));

        Assert.Equal("displayName", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithEmptyEmail_ThrowsArgumentException(string email)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            User.Create("Ola Naji", email, UserRole.Agent));

        Assert.Equal("email", exception.ParamName);
    }

    [Fact]
    public void Create_WithEmailWithoutAtSymbol_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            User.Create("Ola Naji", "ola.example.com", UserRole.Agent));

        Assert.Equal("email", exception.ParamName);
    }

    [Fact]
    public void Create_WithInvalidRole_ThrowsArgumentOutOfRangeException()
    {
        var invalidRole = (UserRole)999;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            User.Create("Ola Naji", "ola@example.com", invalidRole));

        Assert.Equal("role", exception.ParamName);
    }
}
