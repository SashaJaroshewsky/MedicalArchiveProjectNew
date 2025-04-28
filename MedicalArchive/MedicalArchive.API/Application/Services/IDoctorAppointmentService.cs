using MedicalArchive.API.Application.DTOs.DoctorAppointmentDTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IDoctorAppointmentService
    {
        Task<IEnumerable<DoctorAppointmentDto>> GetAllDoctorAppointmentsAsync();
        Task<IEnumerable<DoctorAppointmentDto>> GetDoctorAppointmentsByUserIdAsync(int userId);
        Task<DoctorAppointmentDto> GetDoctorAppointmentByIdAsync(int id);
        Task<DoctorAppointmentDto> CreateDoctorAppointmentAsync(DoctorAppointmentCreateDto appointmentCreateDto);
        Task UpdateDoctorAppointmentAsync(int id, DoctorAppointmentCreateDto appointmentUpdateDto);
        Task DeleteDoctorAppointmentAsync(int id);
    }
}
