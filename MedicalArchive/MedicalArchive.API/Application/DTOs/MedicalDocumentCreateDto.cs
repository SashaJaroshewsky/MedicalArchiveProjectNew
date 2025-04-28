using MedicalArchive.API.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs
{
    public class MedicalDocumentCreateDto
    {
        [Required(ErrorMessage = "ID користувача є обов'язковим")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Назва документа є обов'язковою")]
        public string DocumentName { get; set; }

        [Required(ErrorMessage = "Дата видачі є обов'язковою")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Тип документа є обов'язковим")]
        public DocumentType DocumentType { get; set; }

        public string FilePath { get; set; }

        // ID відповідно до типу документу
        public int? DoctorAppointmentId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? ReferralId { get; set; }
        public int? VaccinationId { get; set; }
    }
}
