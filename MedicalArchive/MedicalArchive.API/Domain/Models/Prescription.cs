namespace MedicalArchive.API.Domain.Models
{
    public class Prescription : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime IssueDate { get; set; }
        public required string MedicationName { get; set; }
        public string? Dosage { get; set; }
        public string? Instructions { get; set; }

        // Навігаційні властивості
        public required virtual User User { get; set; }
        public virtual ICollection<MedicalDocument> Documents { get; set; } = new List<MedicalDocument>();
    }
}
