namespace MedicalArchive.API.Domain.Models
{
    public class Vaccination : BaseEntity
    {
        public int UserId { get; set; }
        public required string VaccineName { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string? Manufacturer { get; set; }
        public int DoseNumber { get; set; }

        // Навігаційні властивості
        public required virtual User User { get; set; }
        public virtual ICollection<MedicalDocument> Documents { get; set; } = new List<MedicalDocument>();
    }
}
