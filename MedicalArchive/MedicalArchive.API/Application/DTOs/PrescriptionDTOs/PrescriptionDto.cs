namespace MedicalArchive.API.Application.DTOs.PrescriptionDTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime IssueDate { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MedicalDocumentDto> Documents { get; set; } = new List<MedicalDocumentDto>();
    }
}
