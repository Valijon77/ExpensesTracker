namespace ExpensesTracker.DTOs;

/// <summary>
/// Used to return DTO from <c>RegisterUser</c> and <c>LoginUser</c> methods.
/// </summary>
public class UserDto // O: Generally, DTO names should be more descriptive.
{
    public required string Username { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}
