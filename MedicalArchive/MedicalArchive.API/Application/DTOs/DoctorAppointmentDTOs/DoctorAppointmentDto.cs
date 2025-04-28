namespace MedicalArchive.API.Application.DTOs.DoctorAppointmentDTOs
{
    public class DoctorAppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorInfo { get; set; }
        public string Complaints { get; set; }
        public string ProceduresDescription { get; set; }
        public string Diagnosis { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MedicalDocumentDto> Documents { get; set; } = new List<MedicalDocumentDto>();
    }
}
