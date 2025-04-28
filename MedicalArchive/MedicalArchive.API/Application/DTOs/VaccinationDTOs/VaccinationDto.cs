namespace MedicalArchive.API.Application.DTOs.VaccinationDTOs
{
    public class VaccinationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string VaccineName { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string Manufacturer { get; set; }
        public int DoseNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MedicalDocumentDto> Documents { get; set; } = new List<MedicalDocumentDto>();
    }
}
