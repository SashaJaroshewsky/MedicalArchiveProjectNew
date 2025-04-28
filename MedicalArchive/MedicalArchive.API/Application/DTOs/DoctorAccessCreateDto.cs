using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs
{
    public class DoctorAccessCreateDto
    {
        [Required(ErrorMessage = "ID користувача є обов'язковим")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "ID лікаря є обов'язковим")]
        public int DoctorId { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
