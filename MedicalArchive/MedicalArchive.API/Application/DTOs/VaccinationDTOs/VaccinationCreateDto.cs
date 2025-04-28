using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs.VaccinationDTOs
{
    public class VaccinationCreateDto
    {
        [Required(ErrorMessage = "ID користувача є обов'язковим")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Назва вакцини є обов'язковою")]
        public string VaccineName { get; set; }

        [Required(ErrorMessage = "Дата вакцинації є обов'язковою")]
        public DateTime VaccinationDate { get; set; }

        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Номер дози є обов'язковим")]
        public int DoseNumber { get; set; }
    }
}
