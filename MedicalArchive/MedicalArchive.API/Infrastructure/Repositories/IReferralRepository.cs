using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IReferralRepository : IBaseRepository<Referral>
    {
        Task<IEnumerable<Referral>> GetByUserIdAsync(int userId);
        Task<Referral?> GetWithDocumentsAsync(int referralId);
    }
}
