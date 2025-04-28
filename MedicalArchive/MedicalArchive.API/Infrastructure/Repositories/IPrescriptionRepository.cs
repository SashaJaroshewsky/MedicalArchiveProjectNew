using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IPrescriptionRepository : IBaseRepository<Prescription>
    {
        Task<IEnumerable<Prescription>> GetByUserIdAsync(int userId);
        Task<Prescription?> GetWithDocumentsAsync(int prescriptionId);
    }
}
