using MedicalArchive.API.Application.DTOs.PrescriptionDTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByUserIdAsync(int userId);
        Task<PrescriptionDto> GetPrescriptionByIdAsync(int id);
        Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionCreateDto);
        Task UpdatePrescriptionAsync(int id, PrescriptionCreateDto prescriptionUpdateDto);
        Task DeletePrescriptionAsync(int id);
    }
}
