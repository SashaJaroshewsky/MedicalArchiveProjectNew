using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs.DoctorAppointmentDTOs
{
    public class DoctorAppointmentCreateDto
    {
        [Required(ErrorMessage = "ID користувача є обов'язковим")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Дата прийому є обов'язковою")]
        public DateTime AppointmentDate { get; set; }

        public string DoctorInfo { get; set; }
        public string Complaints { get; set; }
        public string ProceduresDescription { get; set; }
        public string Diagnosis { get; set; }
    }
}
