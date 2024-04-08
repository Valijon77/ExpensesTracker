namespace ExpensesTracker.Entities;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Country { get; set; }
    public required string FullName { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedDateTime { get; set; } = DateTime.UtcNow;
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; } // O1: apply 'required' keyword depending on how instance is created.
    public required byte[] PasswordSalt { get; set; } // O2: O1
    public List<Expense> Expenses { get; set; } = new();
}
