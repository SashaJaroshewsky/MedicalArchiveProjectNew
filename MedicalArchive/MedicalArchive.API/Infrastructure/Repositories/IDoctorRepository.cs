using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IDoctorRepository : IBaseRepository<Doctor>
    {
        Task<Doctor?> GetByEmailAsync(string email);
        Task<Doctor?> GetDoctorWithAccessesAsync(int doctorId);
        Task<bool> DoctorExistsAsync(string email);
    }
}
