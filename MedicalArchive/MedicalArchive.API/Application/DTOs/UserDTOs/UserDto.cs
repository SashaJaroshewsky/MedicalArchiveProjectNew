using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs.UserDTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище є обов'язковим")]
        public required string LastName { get; set; }

        public required string MiddleName { get; set; }

        [Required(ErrorMessage = "Дата народження є обов'язковою")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Стать є обов'язковою")]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Номер телефону є обов'язковим")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Адреса є обов'язковою")]
        public required string Address { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
