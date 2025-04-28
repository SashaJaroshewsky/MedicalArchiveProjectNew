namespace MedicalArchive.API.Domain.Models
{
    public class DoctorAppointment : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? DoctorInfo { get; set; }
        public string? Complaints { get; set; }
        public string? ProceduresDescription { get; set; }
        public string? Diagnosis { get; set; }

        // Навігаційні властивості
        public required virtual User User { get; set; }
        public virtual ICollection<MedicalDocument> Documents { get; set; } = new List<MedicalDocument>();
    }
}
