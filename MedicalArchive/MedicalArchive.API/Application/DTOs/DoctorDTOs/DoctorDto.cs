using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs.DoctorDTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище є обов'язковим")]
        public required string LastName { get; set; }

        public required string MiddleName { get; set; }

        [Required(ErrorMessage = "Спеціалізація є обов'язковою")]
        public required string Specialization { get; set; }

        [Required(ErrorMessage = "Медичний заклад є обов'язковим")]
        public required string MedicalInstitution { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Номер телефону є обов'язковим")]
        public required string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
