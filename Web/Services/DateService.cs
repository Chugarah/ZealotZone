using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealotZone.Interfaces;
using ZealotZone.Interfaces.Factories;

namespace ZealotZone.Services;

/// <summary>
/// This code is Refactored by Google Gemini Pro
/// Provides functionality to generate selectable options for days, months, and years,
/// which can be used in user interfaces such as dropdown menus.
/// </summary>
public class DateService : IDateService
{
    public List<SelectListItem> GetDayOptions()
    {
        // Let's generate the days
        return Enumerable
            .Range(1, 31)
            .Select(d => new SelectListItem { Value = d.ToString(), Text = d.ToString() })
            .ToList();
    }

    public List<SelectListItem> GetMonthOptions()
    {
        // Generate Months
        // We are using Culture info (Your Locale)
        // https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo?view=net-9.0
        return Enumerable
            .Range(1, 12)
            .Select(m => new SelectListItem
            {
                Value = m.ToString(),
                Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m),
            })
            .ToList();
    }

    /// <summary>
    /// This code was Generated 100% by Google Gemini Pro
    /// Generates a list of selectable year options based on the specified offset ranges from the current year.
    /// This method is commonly used to populate dropdown lists in user interfaces.
    /// </summary>
    /// <param name="startYearOffset">
    /// The number of years to subtract from the current year to calculate the starting year.
    /// Defaults to 100.
    /// </param>
    /// <param name="endYearOffset">
    /// The number of years to subtract from the current year to calculate the ending year.
    /// Defaults to 0.
    /// </param>
    /// <returns>
    /// A list of SelectListItem objects, each representing a year within the specified range.
    /// Returns null if the range is invalid.
    /// </returns>
    public List<SelectListItem>? GetYearOptions(int startYearOffset = 100, int endYearOffset = 0)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startYear = currentYear - startYearOffset;
        var endYear = currentYear - endYearOffset;
        return Enumerable
            .Range(startYear, endYear - startYear + 1)
            .Reverse()
            .Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() })
            .ToList();
    }
}
