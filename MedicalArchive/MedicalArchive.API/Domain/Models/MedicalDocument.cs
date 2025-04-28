using System.Xml.Linq;

namespace MedicalArchive.API.Domain.Models
{
    public class MedicalDocument : BaseEntity
    {
        public int UserId { get; set; }
        public required string DocumentName { get; set; }
        public DateTime IssueDate { get; set; }
        public DocumentType DocumentType { get; set; }
        public string? FilePath { get; set; }

        // ID зв'язаних сутностей (один із них буде заповнений в залежності від типу)
        public int? DoctorAppointmentId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? ReferralId { get; set; }
        public int? VaccinationId { get; set; }

        // Навігаційні властивості
        public required virtual User User { get; set; }
        public virtual DoctorAppointment? DoctorAppointment { get; set; } 
        public virtual Prescription? Prescription { get; set; } 
        public virtual Referral? Referral { get; set; }
        public virtual Vaccination? Vaccination { get; set; }
    }
}
