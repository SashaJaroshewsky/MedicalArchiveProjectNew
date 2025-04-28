using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IDoctorAccessRepository : IBaseRepository<DoctorAccess>
    {
        Task<IEnumerable<DoctorAccess>> GetByUserIdAsync(int userId);
        Task<IEnumerable<DoctorAccess>> GetByDoctorIdAsync(int doctorId);
        Task<DoctorAccess?> GetByUserAndDoctorIdAsync(int userId, int doctorId);
        Task<bool> AccessExistsAsync(int userId, int doctorId);
    }
}
