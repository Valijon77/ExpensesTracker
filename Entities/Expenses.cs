using System.ComponentModel.DataAnnotations;
using ExpensesTracker.DTOs;

namespace ExpensesTracker.Entities;

public class Expenses
{
    [Key]
    public int ExpenseId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow; // [W]: will 'DateTime' behave same as 'string'?: Behaves differently, if value not provided, it doesn't give an error, whereas string does.
    public int WithdrawalAmount { get; set; } // O: use [Required] attribute produce concise code in controller
    public string? Description { get; set; }
    public string Type { get; set; } // O1: change it to use enumeration? O1.1: Change property name to Category
    public string PaymentMethod { get; set; } // O2: O1

    public int UserId { get; set; } // W: will the same functionality be achieved if this line of code is removed
    public User User { get; set; }

    public static explicit operator Expenses(CreateExpenseDto v)
    {
        return new Expenses()
        {
            WithdrawalAmount = v.WithdrawalAmount,
            Description = v.Description,
            Type = v.Type,
            PaymentMethod = v.PaymentMethod
        };
    }

    public static explicit operator Expenses(ExpenseDto v) =>
        new Expenses()
        {
            ExpenseId=v.ExpenseId,
            WithdrawalAmount = v.WithdrawalAmount,
            Description = v.Description,
            Type = v.Type,
            PaymentMethod = v.PaymentMethod
        };
}
