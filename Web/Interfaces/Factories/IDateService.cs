using Microsoft.AspNetCore.Mvc.Rendering;

namespace ZealotZone.Interfaces.Factories;

public interface IDateService
{
    /// <summary>
    /// Gets a list of SelectListItems representing days (1-31).
    /// </summary>
    /// <returns>A list of SelectListItems for days.</returns>
    List<SelectListItem> GetDayOptions();

    /// <summary>
    /// Gets a list of SelectListItems representing months (1-12) with culture-specific names.
    /// </summary>
    /// <returns>A list of SelectListItems for months.</returns>
    List<SelectListItem> GetMonthOptions();

    /// <summary>
    /// Gets a list of SelectListItems representing years within a specified range relative to the current year.
    /// </summary>
    /// <param name="startYearOffset">The number of years before the current year to start the range.</param>
    /// <param name="endYearOffset">The number of years after (or before, if negative) the current year to end the range.</param>
    /// <returns>A list of SelectListItems for years.</returns>
    List<SelectListItem>? GetYearOptions(int startYearOffset = 100, int endYearOffset = 0);
}
