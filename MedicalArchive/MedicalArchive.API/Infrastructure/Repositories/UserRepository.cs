using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithDoctorAccessesAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.DoctorAccesses)
                .ThenInclude(da => da.Doctor)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithMedicalDocumentsAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.MedicalDocuments)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }
    }
}
