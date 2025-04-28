using MedicalArchive.API.Application.DTOs.PrescriptionDTOs;
using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMedicalDocumentRepository _documentRepository;

        public PrescriptionService(
            IPrescriptionRepository prescriptionRepository,
            IUserRepository userRepository,
            IMedicalDocumentRepository documentRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
        {
            var prescriptions = await _prescriptionRepository.GetAllAsync();
            return await MapToDtosAsync(prescriptions);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByUserIdAsync(int userId)
        {
            var prescriptions = await _prescriptionRepository.GetByUserIdAsync(userId);
            return await MapToDtosAsync(prescriptions);
        }

        public async Task<PrescriptionDto> GetPrescriptionByIdAsync(int id)
        {
            var prescription = await _prescriptionRepository.GetWithDocumentsAsync(id);
            if (prescription == null)
                return null;

            return await MapToDtoAsync(prescription);
        }

        public async Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionCreateDto)
        {
            // Перевірка чи існує користувач
            var user = await _userRepository.GetByIdAsync(prescriptionCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {prescriptionCreateDto.UserId} не знайдено");
            }

            var prescription = new Prescription
            {
                UserId = prescriptionCreateDto.UserId,
                IssueDate = prescriptionCreateDto.IssueDate,
                MedicationName = prescriptionCreateDto.MedicationName,
                Dosage = prescriptionCreateDto.Dosage,
                Instructions = prescriptionCreateDto.Instructions,
                User = user
            };

            await _prescriptionRepository.AddAsync(prescription);
            return await MapToDtoAsync(prescription);
        }

        public async Task UpdatePrescriptionAsync(int id, PrescriptionCreateDto prescriptionUpdateDto)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
            {
                throw new KeyNotFoundException($"Рецепт з ID {id} не знайдено");
            }

            // Перевірка чи користувач змінився і чи існує новий користувач
            if (prescription.UserId != prescriptionUpdateDto.UserId)
            {
                var user = await _userRepository.GetByIdAsync(prescriptionUpdateDto.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Користувача з ID {prescriptionUpdateDto.UserId} не знайдено");
                }
            }

            prescription.UserId = prescriptionUpdateDto.UserId;
            prescription.IssueDate = prescriptionUpdateDto.IssueDate;
            prescription.MedicationName = prescriptionUpdateDto.MedicationName;
            prescription.Dosage = prescriptionUpdateDto.Dosage;
            prescription.Instructions = prescriptionUpdateDto.Instructions;

            await _prescriptionRepository.UpdateAsync(prescription);
        }

        public async Task DeletePrescriptionAsync(int id)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
            {
                throw new KeyNotFoundException($"Рецепт з ID {id} не знайдено");
            }

            await _prescriptionRepository.DeleteAsync(id);
        }

        private async Task<PrescriptionDto> MapToDtoAsync(Prescription prescription)
        {
            var user = prescription.User ?? await _userRepository.GetByIdAsync(prescription.UserId);

            var dto = new PrescriptionDto
            {
                Id = prescription.Id,
                UserId = prescription.UserId,
                UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                IssueDate = prescription.IssueDate,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                Instructions = prescription.Instructions,
                CreatedAt = prescription.CreatedAt
            };

            // Додавання документів, якщо вони є
            if (prescription.Documents != null && prescription.Documents.Any())
            {
                foreach (var doc in prescription.Documents)
                {
                    dto.Documents.Add(new MedicalDocumentDto
                    {
                        Id = doc.Id,
                        UserId = doc.UserId,
                        UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                        DocumentName = doc.DocumentName,
                        IssueDate = doc.IssueDate,
                        DocumentType = doc.DocumentType,
                        FilePath = doc.FilePath,
                        RelatedEntityId = prescription.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }
            else
            {
                // Завантаження документів, якщо вони не були включені
                var documents = await _documentRepository.GetByRelatedEntityIdAsync(prescription.Id, DocumentType.Prescription);
                foreach (var doc in documents)
                {
                    dto.Documents.Add(new MedicalDocumentDto
                    {
                        Id = doc.Id,
                        UserId = doc.UserId,
                        UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                        DocumentName = doc.DocumentName,
                        IssueDate = doc.IssueDate,
                        DocumentType = doc.DocumentType,
                        FilePath = doc.FilePath,
                        RelatedEntityId = prescription.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }

            return dto;
        }

        private async Task<IEnumerable<PrescriptionDto>> MapToDtosAsync(IEnumerable<Prescription> prescriptions)
        {
            var result = new List<PrescriptionDto>();
            foreach (var prescription in prescriptions)
            {
                result.Add(await MapToDtoAsync(prescription));
            }
            return result;
        }
    }
}
