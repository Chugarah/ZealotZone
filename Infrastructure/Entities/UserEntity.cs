using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;
public class UserEntity : IdentityUser
{
    // Email doesn't need to be redefining it when your UserEntity is inheriting IdentityUser

    [Required]
    [StringLength(100)]
    public string? FirstName { get; set; }
    [Required]
    [StringLength(100)]
    public string? LastName { get; set; }
}