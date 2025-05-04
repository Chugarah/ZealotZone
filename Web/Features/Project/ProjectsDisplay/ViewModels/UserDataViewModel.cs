using System.ComponentModel.DataAnnotations;

namespace ZealotZone.Features.Project.ProjectsDisplay.ViewModels
{
    public class UserDataViewModel
    {
        [Required]
        public string userEmail { get; set; } = null!;
        [Required]
        public string userFirstName { get; set; } = null!;
        [Required]
        public string userLastName { get; set; } = null!;
    }
}