namespace ExpensesTracker.Helpers;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string TokenKey { get; init; } = string.Empty; // I: 'init' allows immutable object to be initialized during class initialization
}
