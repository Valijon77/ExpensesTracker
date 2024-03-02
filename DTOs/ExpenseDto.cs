namespace ExpensesTracker.DTOs;

/// <summary>
/// Used for the following methods:
/// <list type="bullet">
///     <item>To return DTOs from <c>GetExpenseRecords</c> method.</item>
///     <item>To return DTO from <c>CreateExpenseRecord</c> method.</item>
/// </list>
/// </summary>
public class ExpenseDto
{
    public int ExpenseId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public int WithdrawalAmount { get; set; }
    public string? Description { get; set; }
    public required string Category { get; set; }
    public required string PaymentMethod { get; set; }
}
