namespace ExpensesTracker.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public List<Expenses>? Expenses { get; set; } = new();
}
