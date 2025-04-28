using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs
{
    public class AuthDto
    {
        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        public string Password { get; set; }
    }
}
