namespace FollowUpAgent.Domain.Users;

public sealed class User
{
    private User(Guid id, string displayName, string email, UserRole role)
    {
        Id = id;
        DisplayName = displayName;
        Email = email;
        Role = role;
        IsActive = true;
    }

    public Guid Id { get; }

    public string DisplayName { get; private set; }

    public string Email { get; private set; }

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User is already inactive.");
        }

        IsActive = false;
    }

    public static User Create(string displayName, string email, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.", nameof(displayName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (!email.Contains('@', StringComparison.Ordinal))
        {
            throw new ArgumentException("Email must contain '@'.", nameof(email));
        }

        if (!Enum.IsDefined(role))
        {
            throw new ArgumentOutOfRangeException(nameof(role), role, "User role is invalid.");
        }

        return new User(
            Guid.NewGuid(),
            displayName.Trim(),
            email.Trim(),
            role);
    }
}
