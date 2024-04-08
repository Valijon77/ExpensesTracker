using System.ComponentModel.DataAnnotations;
using ExpensesTracker.Entities;
using ExpensesTracker.Helpers;

namespace ExpensesTracker.DTOs;

/// <summary>
/// Used to receive argument for endpoint <c>UpdateUser</c>.
/// </summary>
public class UserUpdateDto
{
    // public int Id { get; set; } // W: is it safe not to include any validation attributes?

    // [UsernameTaken] // [TODO]: Create a custom validation attribute to check updated username is not taken already.
    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; } // W: does 'required' attribute accept null value?

    public required string Country { get; set; }
    public required string FullName { get; set; }
    public DateTime LastModifiedDateTime { get; set; } = DateTime.UtcNow;
}
