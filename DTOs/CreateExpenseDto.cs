using System.ComponentModel.DataAnnotations;
using ExpensesTracker.Helpers;

namespace ExpensesTracker.DTOs;

/// <summary>
/// Used to receive as an argument for <c>CreateExpenseRecord</c> method.
/// </summary>
public class CreateExpenseDto // O: better if DTOs end with ...Request/...Response
{
    // [O]: decorate with attributes?
    // [O]: add custom validators?
    // W: should I use record types instead of set of properties?

    // W: Is validating here with data annotations good idea? Won't it be better if validation is carried out somewhere else (Fluent Validation)?
    [NotZero]
    [Range(0, int.MaxValue, ErrorMessage = "Withdrawal amount should be positive!")]
    public int WithdrawalAmount { get; set; }

    [StringLength(100, ErrorMessage = "Description can't exceed 100 characters.")]
    public string? Description { get; set; }

    [Available(CategoryOrPaymentMethod = nameof(Category))] // O: Test if it works correctly
    public required string Category { get; set; }

    [Available(CategoryOrPaymentMethod = nameof(PaymentMethod))]
    public required string PaymentMethod { get; set; }
}
