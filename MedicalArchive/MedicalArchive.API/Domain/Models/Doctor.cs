namespace MedicalArchive.API.Domain.Models
{
    public class Doctor : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public required string Specialization { get; set; }
        public required string MedicalInstitution { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }

        // Навігаційні властивості
        public virtual ICollection<DoctorAccess> DoctorAccesses { get; set; } = new List<DoctorAccess>();
    }
}
