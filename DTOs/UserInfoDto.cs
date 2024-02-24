namespace ExpensesTracker.DTOs;

public class UserInfoDto
{
    public string Username { get; set; }
    public IEnumerable<ExpenseDto> Expenses { get; set; }
}
