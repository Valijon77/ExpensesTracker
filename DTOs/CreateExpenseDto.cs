namespace ExpensesTracker.DTOs;

public class CreateExpenseDto
{
    // O: decorate with attributes?
    // O: add custom validators?
    // O: use record types instead of set of properties
    public int WithdrawalAmount { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; }
    public string PaymentMethod { get; set; }
}
