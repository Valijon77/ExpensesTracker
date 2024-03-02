using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.DTOs;

/// <summary>
/// Used to receive as an argument for <c>RegisterUser</c> method.
/// </summary>
public class RegisterDto
{
    // W: does it give same functionality as [Required] attribute?
    [Required]
    public required string Username { get; set; }

    [StringLength(maximumLength: 10, MinimumLength = 6)]
    public required string Password { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Country { get; set; }

    [Required]
    public required string FullName { get; set; }
}
