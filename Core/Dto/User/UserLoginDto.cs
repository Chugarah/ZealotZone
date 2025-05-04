using System.ComponentModel.DataAnnotations;

namespace Core.Dto.User;

public class UserLoginDto
{
    [Required]
    public string? Email { get; init; } = null!;
}