using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IMedicalDocumentRepository : IBaseRepository<MedicalDocument>
    {
        Task<IEnumerable<MedicalDocument>> GetByUserIdAsync(int userId);
        Task<IEnumerable<MedicalDocument>> GetByDocumentTypeAsync(int userId, DocumentType documentType);
        Task<IEnumerable<MedicalDocument>> GetByRelatedEntityIdAsync(int entityId, DocumentType documentType);
    }
}
