using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class DoctorAccessRepository : BaseRepository<DoctorAccess>, IDoctorAccessRepository
    {
        public DoctorAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DoctorAccess>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(da => da.Doctor)
                .Where(da => da.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DoctorAccess>> GetByDoctorIdAsync(int doctorId)
        {
            return await _dbSet
                .Include(da => da.User)
                .Where(da => da.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<DoctorAccess?> GetByUserAndDoctorIdAsync(int userId, int doctorId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(da => da.UserId == userId && da.DoctorId == doctorId);
        }

        public async Task<bool> AccessExistsAsync(int userId, int doctorId)
        {
            return await _dbSet.AnyAsync(da => da.UserId == userId && da.DoctorId == doctorId);
        }
    }
}
