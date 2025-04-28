namespace MedicalArchive.API.Domain.Models
{
    public class Referral : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? ReferralType { get; set; }
        public int ReferralNumber { get; set; }

        // Навігаційні властивості
        public required virtual User User { get; set; }
        public virtual ICollection<MedicalDocument> Documents { get; set; } = new List<MedicalDocument>();
    }
}
