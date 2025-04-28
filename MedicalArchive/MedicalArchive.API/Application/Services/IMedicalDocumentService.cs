using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Application.Services
{
    public interface IMedicalDocumentService
    {
        Task<IEnumerable<MedicalDocumentDto>> GetAllDocumentsAsync();
        Task<IEnumerable<MedicalDocumentDto>> GetDocumentsByUserIdAsync(int userId);
        Task<IEnumerable<MedicalDocumentDto>> GetDocumentsByTypeAsync(int userId, DocumentType documentType);
        Task<IEnumerable<MedicalDocumentDto>> GetDocumentsByRelatedEntityAsync(int entityId, DocumentType documentType);
        Task<MedicalDocumentDto> GetDocumentByIdAsync(int id);
        Task<MedicalDocumentDto> CreateDocumentAsync(MedicalDocumentCreateDto documentCreateDto);
        Task UpdateDocumentAsync(int id, MedicalDocumentCreateDto documentUpdateDto);
        Task DeleteDocumentAsync(int id);
        Task<string> UploadDocumentFileAsync(IFormFile file);
    }
}
