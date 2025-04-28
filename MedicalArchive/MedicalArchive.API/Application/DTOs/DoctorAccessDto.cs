namespace MedicalArchive.API.Application.DTOs
{
    public class DoctorAccessDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public string? UserFullName { get; set; }
        public string? DoctorFullName { get; set; }
        public DateTime GrantedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
