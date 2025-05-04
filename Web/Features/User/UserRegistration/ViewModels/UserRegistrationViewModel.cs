using System.ComponentModel.DataAnnotations;

namespace ZealotZone.Features.User.UserRegistration.ViewModels;

public class UserRegistrationViewModel
{
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please provide a valid full name.")]
    [RegularExpression(
        @"^[A-Za-z]+ [A-Za-z]+$",
        ErrorMessage = "Full name must be in the format 'FirstName LastName'."
    )]
    [Display(Name = "Full Name", Prompt = "Full Name")]
    public string FullName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Please provide a valid email address.")]
    // This is needed for Client-side validation
    [RegularExpression(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        ErrorMessage = "Please provide proper email address."
    )]
    [Display(Name = "Email Address", Prompt = "Email address")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please provide a valid password.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(50, ErrorMessage = "Password must be at most 50 characters long.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=\-[\]{};':""\\|,.<>/?~`])[\w!@#$%^&*()_+=\-[\]{};':""\\|,.<>/?~`]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
    )]
    [Display(Name = "Password", Prompt = "Password")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "You will not escape your Doom!")]
    [Display(Name = "Your soul is mine!, click to accept.")]
    public bool AcceptTerms { get; set; }
}
