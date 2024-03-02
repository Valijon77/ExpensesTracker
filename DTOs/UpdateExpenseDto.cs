using System.ComponentModel.DataAnnotations;
using ExpensesTracker.Helpers;

namespace ExpensesTracker.DTOs;

/// <summary>
/// To receive as an argument for <c>UpdateExpenseRecord</c> method.
/// </summary>
public class UpdateExpenseDto
{
    [Required]
    public int ExpenseId { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    [NotZero]
    [Range(0, int.MaxValue, ErrorMessage = "Withdrawal amount should be positive!")]
    public int WithdrawalAmount { get; set; }

    [StringLength(100, ErrorMessage = "Description can't exceed 100 characters.")]
    public string? Description { get; set; }

    [Available(CategoryOrPaymentMethod = nameof(Category))]
    public required string Category { get; set; }

    [Available(CategoryOrPaymentMethod = nameof(PaymentMethod))]
    public required string PaymentMethod { get; set; }
}
