using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class MedicalDocumentService : IMedicalDocumentService
    {
        private readonly IMedicalDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDoctorAppointmentRepository _appointmentRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IReferralRepository _referralRepository;
        private readonly IVaccinationRepository _vaccinationRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MedicalDocumentService(
            IMedicalDocumentRepository documentRepository,
            IUserRepository userRepository,
            IDoctorAppointmentRepository appointmentRepository,
            IPrescriptionRepository prescriptionRepository,
            IReferralRepository referralRepository,
            IVaccinationRepository vaccinationRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
            _prescriptionRepository = prescriptionRepository;
            _referralRepository = referralRepository;
            _vaccinationRepository = vaccinationRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<MedicalDocumentDto>> GetAllDocumentsAsync()
        {
            var documents = await _documentRepository.GetAllAsync();
            return await MapToDtosAsync(documents);
        }

        public async Task<IEnumerable<MedicalDocumentDto>> GetDocumentsByUserIdAsync(int userId)
        {
            var documents = await _documentRepository.GetByUserIdAsync(userId);
            return await MapToDtosAsync(documents);
        }

        public async Task<IEnumerable<MedicalDocumentDto>> GetDocumentsByTypeAsync(int userId, DocumentType documentType)
        {
            var documents = await _documentRepository.GetByDocumentTypeAsync(userId, documentType);
            return await MapToDtosAsync(documents);
        }

        public async Task<IEnumerable<MedicalDocumentDto>> GetDocumentsByRelatedEntityAsync(int entityId, DocumentType documentType)
        {
            var documents = await _documentRepository.GetByRelatedEntityIdAsync(entityId, documentType);
            return await MapToDtosAsync(documents);
        }

        public async Task<MedicalDocumentDto> GetDocumentByIdAsync(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                return null;

            return await MapToDtoAsync(document);
        }

        public async Task<MedicalDocumentDto> CreateDocumentAsync(MedicalDocumentCreateDto documentCreateDto)
        {
            // Перевірка чи існує користувач
            var user = await _userRepository.GetByIdAsync(documentCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {documentCreateDto.UserId} не знайдено");
            }

            // Перевірка пов'язаної сутності в залежності від типу документа
            await ValidateRelatedEntityAsync(documentCreateDto);

            var document = new MedicalDocument
            {
                UserId = documentCreateDto.UserId,
                DocumentName = documentCreateDto.DocumentName,
                IssueDate = documentCreateDto.IssueDate,
                DocumentType = documentCreateDto.DocumentType,
                FilePath = documentCreateDto.FilePath,
                DoctorAppointmentId = documentCreateDto.DoctorAppointmentId,
                PrescriptionId = documentCreateDto.PrescriptionId,
                ReferralId = documentCreateDto.ReferralId,
                VaccinationId = documentCreateDto.VaccinationId,
                User = user
            };

            await _documentRepository.AddAsync(document);
            return await MapToDtoAsync(document);
        }

        public async Task UpdateDocumentAsync(int id, MedicalDocumentCreateDto documentUpdateDto)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
            {
                throw new KeyNotFoundException($"Документ з ID {id} не знайдено");
            }

            // Перевірка чи користувач змінився і чи існує новий користувач
            if (document.UserId != documentUpdateDto.UserId)
            {
                var user = await _userRepository.GetByIdAsync(documentUpdateDto.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Користувача з ID {documentUpdateDto.UserId} не знайдено");
                }
            }

            // Перевірка пов'язаної сутності в залежності від типу документа
            await ValidateRelatedEntityAsync(documentUpdateDto);

            document.UserId = documentUpdateDto.UserId;
            document.DocumentName = documentUpdateDto.DocumentName;
            document.IssueDate = documentUpdateDto.IssueDate;
            document.DocumentType = documentUpdateDto.DocumentType;
            document.FilePath = documentUpdateDto.FilePath ?? document.FilePath;
            document.DoctorAppointmentId = documentUpdateDto.DoctorAppointmentId;
            document.PrescriptionId = documentUpdateDto.PrescriptionId;
            document.ReferralId = documentUpdateDto.ReferralId;
            document.VaccinationId = documentUpdateDto.VaccinationId;

            await _documentRepository.UpdateAsync(document);
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
            {
                throw new KeyNotFoundException($"Документ з ID {id} не знайдено");
            }

            // Видалення файлу, якщо він існує
            if (!string.IsNullOrEmpty(document.FilePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, document.FilePath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await _documentRepository.DeleteAsync(id);
        }

        public async Task<string> UploadDocumentFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Файл не завантажено або він порожній");
            }

            // Перевірка розширення файлу
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Недопустимий тип файлу. Дозволені типи: " + string.Join(", ", allowedExtensions));
            }

            // Створення директорії для завантажень, якщо вона не існує
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Генерація унікального імені файлу
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Збереження файлу
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Повернення відносного шляху до файлу
            return $"/uploads/{uniqueFileName}";
        }

        private async Task ValidateRelatedEntityAsync(MedicalDocumentCreateDto document)
        {
            switch (document.DocumentType)
            {
                case DocumentType.DoctorAppointment:
                    if (document.DoctorAppointmentId.HasValue)
                    {
                        var appointment = await _appointmentRepository.GetByIdAsync(document.DoctorAppointmentId.Value);
                        if (appointment == null)
                        {
                            throw new KeyNotFoundException($"Прийом з ID {document.DoctorAppointmentId} не знайдено");
                        }
                    }
                    break;
                case DocumentType.Prescription:
                    if (document.PrescriptionId.HasValue)
                    {
                        var prescription = await _prescriptionRepository.GetByIdAsync(document.PrescriptionId.Value);
                        if (prescription == null)
                        {
                            throw new KeyNotFoundException($"Рецепт з ID {document.PrescriptionId} не знайдено");
                        }
                    }
                    break;
                case DocumentType.Referral:
                    if (document.ReferralId.HasValue)
                    {
                        var referral = await _referralRepository.GetByIdAsync(document.ReferralId.Value);
                        if (referral == null)
                        {
                            throw new KeyNotFoundException($"Направлення з ID {document.ReferralId} не знайдено");
                        }
                    }
                    break;
                case DocumentType.Vaccination:
                    if (document.VaccinationId.HasValue)
                    {
                        var vaccination = await _vaccinationRepository.GetByIdAsync(document.VaccinationId.Value);
                        if (vaccination == null)
                        {
                            throw new KeyNotFoundException($"Вакцинацію з ID {document.VaccinationId} не знайдено");
                        }
                    }
                    break;
            }
        }

        private async Task<MedicalDocumentDto> MapToDtoAsync(MedicalDocument document)
        {
            var user = document.User ?? await _userRepository.GetByIdAsync(document.UserId);

            int? relatedEntityId = null;
            switch (document.DocumentType)
            {
                case DocumentType.DoctorAppointment:
                    relatedEntityId = document.DoctorAppointmentId;
                    break;
                case DocumentType.Prescription:
                    relatedEntityId = document.PrescriptionId;
                    break;
                case DocumentType.Referral:
                    relatedEntityId = document.ReferralId;
                    break;
                case DocumentType.Vaccination:
                    relatedEntityId = document.VaccinationId;
                    break;
            }

            return new MedicalDocumentDto
            {
                Id = document.Id,
                UserId = document.UserId,
                UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                DocumentName = document.DocumentName,
                IssueDate = document.IssueDate,
                DocumentType = document.DocumentType,
                FilePath = document.FilePath,
                RelatedEntityId = relatedEntityId,
                CreatedAt = document.CreatedAt
            };
        }

        private async Task<IEnumerable<MedicalDocumentDto>> MapToDtosAsync(IEnumerable<MedicalDocument> documents)
        {
            var result = new List<MedicalDocumentDto>();
            foreach (var document in documents)
            {
                result.Add(await MapToDtoAsync(document));
            }
            return result;
        }
    }
}
