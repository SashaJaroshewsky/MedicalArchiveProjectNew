using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class DoctorAppointmentRepository : BaseRepository<DoctorAppointment>, IDoctorAppointmentRepository
    {
        public DoctorAppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DoctorAppointment>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(da => da.UserId == userId)
                .OrderByDescending(da => da.AppointmentDate)
                .ToListAsync();
        }

        public async Task<DoctorAppointment?> GetWithDocumentsAsync(int appointmentId)
        {
            return await _dbSet
                .Include(da => da.Documents)
                .FirstOrDefaultAsync(da => da.Id == appointmentId);
        }
    }
}
