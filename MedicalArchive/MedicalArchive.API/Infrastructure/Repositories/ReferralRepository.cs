using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class ReferralRepository : BaseRepository<Referral>, IReferralRepository
    {
        public ReferralRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Referral>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.IssueDate)
                .ToListAsync();
        }

        public async Task<Referral?> GetWithDocumentsAsync(int referralId)
        {
            return await _dbSet
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(r => r.Id == referralId);
        }
    }
}
