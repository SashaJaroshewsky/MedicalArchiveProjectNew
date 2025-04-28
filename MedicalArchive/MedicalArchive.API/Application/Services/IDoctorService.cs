using MedicalArchive.API.Application.DTOs.DoctorDTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto> GetDoctorByIdAsync(int id);
        Task<DoctorDto> GetDoctorByEmailAsync(string email);
        Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto doctorCreateDto);
        Task UpdateDoctorAsync(int id, DoctorDto doctorDto);
        Task DeleteDoctorAsync(int id);
    }
}
