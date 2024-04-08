using System.ComponentModel.DataAnnotations;
using ExpensesTracker.Data;

namespace ExpensesTracker.Helpers;

public class UsernameTakenAttribute : ValidationAttribute
{
    // public int UserId { get; set; } = UserId;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            DataContext? context = validationContext.GetRequiredService<DataContext>();
            var user = context.Users.SingleOrDefault(x => x.Username == (string)value);

            if (user != null)
            {
                return new ValidationResult(
                    ErrorMessage ?? "Your updated username is already taken"
                );
            }
        }

        return ValidationResult.Success;
    }
}
