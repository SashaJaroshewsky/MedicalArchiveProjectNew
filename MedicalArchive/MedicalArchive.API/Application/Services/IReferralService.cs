using MedicalArchive.API.Application.DTOs.ReferralDTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IReferralService
    {
        Task<IEnumerable<ReferralDto>> GetAllReferralsAsync();
        Task<IEnumerable<ReferralDto>> GetReferralsByUserIdAsync(int userId);
        Task<ReferralDto> GetReferralByIdAsync(int id);
        Task<ReferralDto> CreateReferralAsync(ReferralCreateDto referralCreateDto);
        Task UpdateReferralAsync(int id, ReferralCreateDto referralUpdateDto);
        Task DeleteReferralAsync(int id);
    }
}
