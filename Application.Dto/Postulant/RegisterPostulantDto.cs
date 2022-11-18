using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Postulant;

public class RegisterPostulantDto
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(4)] public string Password { get; set; } = string.Empty;
    [Required, Compare("Password")] public string ConfirmPassword { get; set; } = string.Empty;
}