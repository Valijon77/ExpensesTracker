using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesTracker.Helpers;

public class AvailableAttribute : ValidationAttribute
{
    private IEnumerable<string> availableCategories = new List<string>()
    {
        "food",
        "purchase",
        "service",
        "mortgage",
        "taxes",
        "loan",
        "charity",
        "transport",
        "other",
    };

    private IEnumerable<string> availablePaymentMethods = new List<string>()
    {
        "online",
        "cash",
        "crypto"
    };

    public string? CategoryOrPaymentMethod { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (!CategoryOrPaymentMethod.IsNullOrEmpty() && value != null)
        {
            var availableList = CategoryOrPaymentMethod switch
            {
                "Category" => availableCategories,
                "PaymentMethod" => availablePaymentMethods,
                _
                    => throw new Exception(
                        "Annotation should specify one of the two: Category or PaymentMethod!"
                    )
            };

            bool isValid = availableList.Any(x => x == value.ToString());

            if (!isValid)
            {
                return new ValidationResult(
                    ErrorMessage
                        ?? $"Specified '{value}' doesn't correspond to any available types in {CategoryOrPaymentMethod} list."
                );
            }
        }

        return ValidationResult.Success;
    }
}
