using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ZealotZone.Areas.Registrations.ViewModels;

public class BirthDateInputViewModel : IValidatableObject
{
    [Display(Name = "Day", Prompt = "Day")]
    [Range(1, 31, ErrorMessage = "Day must be between 1 and 31.")]
    public int? Day { get; set; }

    [Display(Name = "Month", Prompt = "Month")]
    // Consider using an enum or dropdown for months for better validation and UX
    public string? Month { get; set; } // Assuming a month is entered as a number string e.g., "1", "12"

    [Display(Name = "Year", Prompt = "Year")]
    // Add Range if needed, e.g., [Range(1900, 2025)]
    public int? Year { get; set; }

    /// <summary>
    /// https://stackoverflow.com/questions/3400542/how-do-i-use-ivalidatableobject
    /// Validates groups of entities, like day, month and year so we can
    /// create a proper date format :)
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // We should only validate if all parts are filled.
        var dayData = Day.HasValue;
        var monthData = !string.IsNullOrWhiteSpace(Month);
        var yearData = Year.HasValue;

        // Check if we have provided one of them
        var partialProvision = dayData || monthData || yearData;
        // Check if we have provided all of them
        var allData = dayData && monthData && yearData;

        // https://stackoverflow.com/questions/231893/what-does-yield-break-do-in-c
        // If we have provided one of them but not all of them, return an error
        if (!partialProvision || allData)
            yield break;
        yield return new ValidationResult(
            "Please provide all parts of the birthdate.",
            [nameof(Day), nameof(Month), nameof(Year)]
        );

        // If all parts are provided, proceed with date validity check
        if (!allData)
            yield break;
        ValidationResult? dateValidationError = null;
        int monthInt;

        // Try to parse the month string to an integer
        // Google Gemini Pro helped me to solve this parsing code block
        if (
            !int.TryParse(Month, NumberStyles.Integer, CultureInfo.InvariantCulture, out monthInt)
            || monthInt < 1
            || monthInt > 12
        )
        {
            dateValidationError = new ValidationResult(
                "Month must be a valid number between 1 and 12.",
                [nameof(Month)]
            );
        }
        else
        {
            // If the month is valid, check if the combination of Day, Month, and Year forms a valid date.
            try
            {
                // Attempt to create a DateTime object.
                // This will throw an ArgumentOutOfRangeException for invalid dates (e.g., Feb 14th).
                if (Day != null && Year != null)
                {
                    var date = new DateTime(Year.Value, monthInt, Day.Value);
                }
                // TODO: Implement save service for add member date
            }
            catch (ArgumentOutOfRangeException)
            {
                // If DateTime creation fails, the date is invalid.
                // Target the error message to the specific fields involved or the overall concept
                dateValidationError = new ValidationResult(
                    "The selected Day, Month, and Year do not form a valid date.",
                    [nameof(Day), nameof(Month), nameof(Year)]
                );
            }
        }
        // If there was an error, yield it.
        if (dateValidationError != null)
        {
            yield return dateValidationError;
        }
    }
}
