using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;
public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    [StringLength(100)]
    public string? FirstName { get; init; }

    [ProtectedPersonalData]
    [StringLength(100)]
    public string? LastName { get; init; } = null!;
}