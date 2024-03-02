namespace ExpensesTracker.DTOs;

/// <summary>
/// Only used to return DTO from <c>GetUserByUsername</c> method.
/// </summary>
public class UserInfoDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Country { get; set; }
    public required string FullName { get; set; }
    public DateTime MemberSince { get; set; } // [O]: Add required AutoMapper configuration to map correctly (User -> UserInfoDto)
    public DateTime ProfileUpdated { get; set; } // W: should I need to return this?
    public IEnumerable<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>(); // [W]: what does it return? An empty list? : Yes
}
