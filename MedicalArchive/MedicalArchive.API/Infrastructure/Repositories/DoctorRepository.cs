using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Doctor?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<Doctor?> GetDoctorWithAccessesAsync(int doctorId)
        {
            return await _dbSet
                .Include(d => d.DoctorAccesses)
                .ThenInclude(da => da.User)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
        }

        public async Task<bool> DoctorExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(d => d.Email == email);
        }
    }
}
