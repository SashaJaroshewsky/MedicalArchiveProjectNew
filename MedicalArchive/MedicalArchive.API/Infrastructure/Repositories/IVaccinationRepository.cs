using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IVaccinationRepository : IBaseRepository<Vaccination>
    {
        Task<IEnumerable<Vaccination>> GetByUserIdAsync(int userId);
        Task<Vaccination?> GetWithDocumentsAsync(int vaccinationId);
    }
}
