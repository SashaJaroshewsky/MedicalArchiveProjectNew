using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Application.DTOs
{
    public class MedicalDocumentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string DocumentName { get; set; }
        public DateTime IssueDate { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeName => DocumentType.ToString();
        public string FilePath { get; set; }
        public int? RelatedEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
