namespace ZealotZone.ViewModels.Forms;

/// <summary>
/// Got assisted by Google Gemini Pro AI to create this ViewModel
/// Represents a view model for selecting a date of birth, providing
/// lists of days, months, and years, as well as selected values for
/// the day, month, and year.
/// </summary>
public class BirthSelectionViewModel
{
    // Creating a list of days
    public List<int>? Days { get; set; } = [];
    public List<KeyValuePair<int, string>>? Months { get; set; } = [];
    public List<int>? Years { get; set; } = [];

    // Labels
    public string DayFieldName { get; set; } = "Day";
    public string MonthFieldName { get; set; } = "Month";
    public string YearFieldName { get; set; } = "Year";

    // Selected Values
    public int SelectedDay { get; set; }
    public int SelectedMonth { get; set; }
    public int SelectedYear { get; set; }
}
