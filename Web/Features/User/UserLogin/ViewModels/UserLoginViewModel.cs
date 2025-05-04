using System.ComponentModel.DataAnnotations;

namespace ZealotZone.Features.User.UserLogin.ViewModels
{
    public class UserLoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please provide a valid email address.")]
        // This is needed for Client-side validation
        [RegularExpression(
            @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
            ErrorMessage = "Please provide proper email address."
        )]
        [Display(Name = "Email Address", Prompt = "Email address")]
        public string Email { get; init; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please provide a valid password.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(50, ErrorMessage = "Password must be at most 50 characters long.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=\-[\]{};':""\\|,.<>/?~`])[\w!@#$%^&*()_+=\-[\]{};':""\\|,.<>/?~`]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
        )]
        [Display(Name = "Password", Prompt = "Password")]
        public string Password { get; init; } = null!;
    }
}
