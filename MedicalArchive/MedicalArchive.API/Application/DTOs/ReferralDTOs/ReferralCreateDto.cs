using System.ComponentModel.DataAnnotations;

namespace MedicalArchive.API.Application.DTOs.ReferralDTOs
{
    public class ReferralCreateDto
    {
        [Required(ErrorMessage = "ID користувача є обов'язковим")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Дата видачі є обов'язковою")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Термін дії є обов'язковим")]
        public DateTime ExpirationDate { get; set; }

        public string ReferralType { get; set; }

        [Required(ErrorMessage = "Номер напрвлення є обов'язковим")]
        public int ReferralNumber { get; set; }
    }
}
