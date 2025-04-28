namespace MedicalArchive.API.Domain.Models
{
    public class DoctorAccess : BaseEntity
    {
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }

        // Навігаційні властивості
        public required virtual User User { get; set; }
        public required virtual Doctor Doctor { get; set; }
    }
}
