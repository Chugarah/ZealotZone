using System.ComponentModel.DataAnnotations;

namespace Core.Dto.User;

public class UserInsertDto
{
    [Required]
    public string Email { get; init; } = null!;
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
}