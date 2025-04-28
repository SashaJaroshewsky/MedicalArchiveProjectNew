using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs.PrescriptionDTOs
{
    public class PrescriptionCreateDto
    {
        [Required(ErrorMessage = "ID користувача є обов'язковим")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Дата видачі є обов'язковою")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Назва препарату є обов'язковою")]
        public string MedicationName { get; set; }

        public string Dosage { get; set; }
        public string Instructions { get; set; }
    }
}
