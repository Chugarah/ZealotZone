using System.ComponentModel.DataAnnotations;

namespace Core.Dto.User;

public class UserInsertDto
{
    [StringLength(100)]
    public string FirstName { get; init; } = null!;
    [StringLength(100)]
    public string LastName { get; init; } = null!;
}