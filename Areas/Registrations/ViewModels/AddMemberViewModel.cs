using System.ComponentModel.DataAnnotations;

namespace ZealotZone.Areas.Registrations.ViewModels;

public class AddMemberViewModel
{
    [DataType(DataType.Upload)]
    [UIHint("AvatarUpload")]
    [Display(Name = "Avatar", Prompt = "Avatar")]
    public IFormFile? Avatar { get; set; }
    public BirthDateInputViewModel? BirthDate { get; set; }

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please enter a valid first name.")]
    [MinLength(3, ErrorMessage = "First name must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "First name must be at most 50 characters long.")]
    [RegularExpression(
        @"^[a-zA-ZäöåÄÖÅ0-9-\s]+$",
        ErrorMessage = "First name can only contain letters and spaces."
    )]
    [Display(Name = "First Name", Prompt = "First name")]
    public string FirstName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please enter a valid last name.")]
    [MinLength(3, ErrorMessage = "Last name must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "Last name must be at most 50 characters long.")]
    [RegularExpression(
        @"^[a-zA-ZäöåÄÖÅ0-9-\s]+$",
        ErrorMessage = "Last name can only contain letters and spaces."
    )]
    [Display(Name = "Last Name", Prompt = "Last name")]
    public string LastName { get; set; } = null!;

    [DataType(DataType.EmailAddress)] // Keep for server-side validation & semantics
    [Required(ErrorMessage = "Please provide a valid email address.")]
    // This is needed for Client-side validation
    [RegularExpression(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        ErrorMessage = "Please provide proper email address."
    )]
    [Display(Name = "Email Address", Prompt = "Email address")]
    public string EmailAddress { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number", Prompt = "Phone number")]
    // Code Generated with Google Gemini Pro
    [RegularExpression(
        @"^(?:\+46)?[0-9]{7,13}$", // Allows +46 prefix optionally
        ErrorMessage = "Phone number must be 7 to 13 digits, optionally starting with +46."
    )]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Job Title", Prompt = "Job title")]
    [MinLength(3, ErrorMessage = "Job title must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "Job title must be at most 50 characters long.")]
    [RegularExpression(
        @"^[a-zA-ZäöåÄÖÅ0-9-\s]+$",
        ErrorMessage = "Job title can only contain letters and spaces."
    )]
    public string? JobTitle { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Adress", Prompt = "Adress")]
    [MinLength(10, ErrorMessage = "Adress must be at least 10 characters long.")]
    [MaxLength(100, ErrorMessage = "Adress must be at most 100 characters long.")]
    [Required(ErrorMessage = "Please enter a valid Adress.")]
    [RegularExpression(
        @"^[a-zA-ZäöåÄÖÅ0-9-\s]+$",
        ErrorMessage = "Adress can only contain letters, numbers, and spaces."
    )]
    public string Adress { get; set; } = null!;
}
