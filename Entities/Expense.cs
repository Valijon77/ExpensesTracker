using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Entities;

public class Expense
{
    [Key]
    public int ExpenseId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // [W]: will 'DateTime' behave same as 'string'?: Behaves differently, if value not provided, it doesn't give an error, whereas string does.
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    public int WithdrawalAmount { get; set; }
    public string? Description { get; set; }
    public required string Category { get; set; } // O1: change it to use enumeration? [O1.1]: Change property name to Category
    public required string PaymentMethod { get; set; } // O2: O1

    // I: (non-)nullability of FK property will determine if relationship with principal is optional or required
    public int UserId { get; set; } // W: will the same functionality be achieved if this line of code is removed: Yes! EF creates shadow FK called 'UserId' by convention.
    public User User { get; set; } = null!;
}
