using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public class MedicalDocumentRepository : BaseRepository<MedicalDocument>, IMedicalDocumentRepository
    {
        public MedicalDocumentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MedicalDocument>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(md => md.UserId == userId)
                .OrderByDescending(md => md.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalDocument>> GetByDocumentTypeAsync(int userId, DocumentType documentType)
        {
            return await _dbSet
                .Where(md => md.UserId == userId && md.DocumentType == documentType)
                .OrderByDescending(md => md.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalDocument>> GetByRelatedEntityIdAsync(int entityId, DocumentType documentType)
        {
            // Вибір відповідного ID залежно від типу документа
            return documentType switch
            {
                DocumentType.DoctorAppointment => await _dbSet
                    .Where(md => md.DoctorAppointmentId == entityId)
                    .OrderByDescending(md => md.IssueDate)
                    .ToListAsync(),

                DocumentType.Prescription => await _dbSet
                    .Where(md => md.PrescriptionId == entityId)
                    .OrderByDescending(md => md.IssueDate)
                    .ToListAsync(),

                DocumentType.Referral => await _dbSet
                    .Where(md => md.ReferralId == entityId)
                    .OrderByDescending(md => md.IssueDate)
                    .ToListAsync(),

                DocumentType.Vaccination => await _dbSet
                    .Where(md => md.VaccinationId == entityId)
                    .OrderByDescending(md => md.IssueDate)
                    .ToListAsync(),

                _ => new List<MedicalDocument>()
            };
        }
    }
}
