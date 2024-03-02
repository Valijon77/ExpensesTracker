using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.DTOs;

/// <summary>
/// Used to receive as an argument for endpoint <c>LoginUser</c>
/// </summary>
public class LoginDto
{
    [Required] // I: preventing null and empty string
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; } // I: adding 'required' keyword doesn't prevent from sending an empty string.
}
