using MedicalArchive.API.Application.DTOs.VaccinationDTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IVaccinationService
    {
        Task<IEnumerable<VaccinationDto>> GetAllVaccinationsAsync();
        Task<IEnumerable<VaccinationDto>> GetVaccinationsByUserIdAsync(int userId);
        Task<VaccinationDto> GetVaccinationByIdAsync(int id);
        Task<VaccinationDto> CreateVaccinationAsync(VaccinationCreateDto vaccinationCreateDto);
        Task UpdateVaccinationAsync(int id, VaccinationCreateDto vaccinationUpdateDto);
        Task DeleteVaccinationAsync(int id);
    }
}
