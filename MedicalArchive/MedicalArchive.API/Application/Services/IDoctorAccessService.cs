using MedicalArchive.API.Application.DTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IDoctorAccessService
    {
        Task<IEnumerable<DoctorAccessDto>> GetAllDoctorAccessesAsync();
        Task<IEnumerable<DoctorAccessDto>> GetDoctorAccessesByUserIdAsync(int userId);
        Task<IEnumerable<DoctorAccessDto>> GetDoctorAccessesByDoctorIdAsync(int doctorId);
        Task<DoctorAccessDto> GetDoctorAccessByIdAsync(int id);
        Task<DoctorAccessDto> CreateDoctorAccessAsync(DoctorAccessCreateDto accessCreateDto);
        Task DeleteDoctorAccessAsync(int id);
    }
}
