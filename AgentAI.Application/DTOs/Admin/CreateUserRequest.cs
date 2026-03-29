using System.ComponentModel.DataAnnotations;
using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Admin;

public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
