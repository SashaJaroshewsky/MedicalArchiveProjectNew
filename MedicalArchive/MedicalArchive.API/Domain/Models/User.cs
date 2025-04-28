namespace MedicalArchive.API.Domain.Models
{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required string PasswordHash { get; set; }

        // Навігаційні властивості
        public virtual ICollection<DoctorAppointment> DoctorAppointments { get; set; } = new List<DoctorAppointment>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();
        public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
        public virtual ICollection<MedicalDocument> MedicalDocuments { get; set; } = new List<MedicalDocument>();
        public virtual ICollection<DoctorAccess> DoctorAccesses { get; set; } = new List<DoctorAccess>();
    }
}
