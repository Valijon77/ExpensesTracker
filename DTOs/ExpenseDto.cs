using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.DTOs;

public class ExpenseDto
{
    [Required]
    public int ExpenseId { get; set; }
    public int WithdrawalAmount { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; }
    public string PaymentMethod { get; set; }
}
