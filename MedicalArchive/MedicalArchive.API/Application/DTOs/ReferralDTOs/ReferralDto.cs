namespace MedicalArchive.API.Application.DTOs.ReferralDTOs
{
    public class ReferralDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ReferralType { get; set; }
        public int ReferralNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MedicalDocumentDto> Documents { get; set; } = new List<MedicalDocumentDto>();
    }
}
