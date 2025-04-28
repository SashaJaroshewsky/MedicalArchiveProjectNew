using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class PrescriptionRepository : BaseRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Prescription>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.IssueDate)
                .ToListAsync();
        }

        public async Task<Prescription?> GetWithDocumentsAsync(int prescriptionId)
        {
            return await _dbSet
                .Include(p => p.Documents)
                .FirstOrDefaultAsync(p => p.Id == prescriptionId);
        }
    }
}
