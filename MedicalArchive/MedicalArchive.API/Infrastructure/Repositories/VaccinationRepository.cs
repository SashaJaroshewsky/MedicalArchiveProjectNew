using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class VaccinationRepository : BaseRepository<Vaccination>, IVaccinationRepository
    {
        public VaccinationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vaccination>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.VaccinationDate)
                .ToListAsync();
        }

        public async Task<Vaccination?> GetWithDocumentsAsync(int vaccinationId)
        {
            return await _dbSet
                .Include(v => v.Documents)
                .FirstOrDefaultAsync(v => v.Id == vaccinationId);
        }
    }
}
