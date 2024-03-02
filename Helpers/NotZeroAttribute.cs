using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Helpers;

public class NotZeroAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            int valueDecimal = int.Parse(value.ToString()!);
            if (valueDecimal == 0)
            {
                return new ValidationResult(
                    ErrorMessage ?? $"Expenditure amount should be greater than 0!"
                );
            }
        }

        return ValidationResult.Success;
    }
}
