using MedicalArchive.API.Application.DTOs.VaccinationDTOs;
using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class VaccinationService : IVaccinationService
    {
        private readonly IVaccinationRepository _vaccinationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMedicalDocumentRepository _documentRepository;

        public VaccinationService(
            IVaccinationRepository vaccinationRepository,
            IUserRepository userRepository,
            IMedicalDocumentRepository documentRepository)
        {
            _vaccinationRepository = vaccinationRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
        }

        public async Task<IEnumerable<VaccinationDto>> GetAllVaccinationsAsync()
        {
            var vaccinations = await _vaccinationRepository.GetAllAsync();
            return await MapToDtosAsync(vaccinations);
        }

        public async Task<IEnumerable<VaccinationDto>> GetVaccinationsByUserIdAsync(int userId)
        {
            var vaccinations = await _vaccinationRepository.GetByUserIdAsync(userId);
            return await MapToDtosAsync(vaccinations);
        }

        public async Task<VaccinationDto> GetVaccinationByIdAsync(int id)
        {
            var vaccination = await _vaccinationRepository.GetWithDocumentsAsync(id);
            if (vaccination == null)
                return null;

            return await MapToDtoAsync(vaccination);
        }

        public async Task<VaccinationDto> CreateVaccinationAsync(VaccinationCreateDto vaccinationCreateDto)
        {
            // Перевірка чи існує користувач
            var user = await _userRepository.GetByIdAsync(vaccinationCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {vaccinationCreateDto.UserId} не знайдено");
            }

            var vaccination = new Vaccination
            {
                UserId = vaccinationCreateDto.UserId,
                VaccineName = vaccinationCreateDto.VaccineName,
                VaccinationDate = vaccinationCreateDto.VaccinationDate,
                Manufacturer = vaccinationCreateDto.Manufacturer,
                DoseNumber = vaccinationCreateDto.DoseNumber,
                User = user
            };

            await _vaccinationRepository.AddAsync(vaccination);
            return await MapToDtoAsync(vaccination);
        }

        public async Task UpdateVaccinationAsync(int id, VaccinationCreateDto vaccinationUpdateDto)
        {
            var vaccination = await _vaccinationRepository.GetByIdAsync(id);
            if (vaccination == null)
            {
                throw new KeyNotFoundException($"Вакцинацію з ID {id} не знайдено");
            }

            // Перевірка чи користувач змінився і чи існує новий користувач
            if (vaccination.UserId != vaccinationUpdateDto.UserId)
            {
                var user = await _userRepository.GetByIdAsync(vaccinationUpdateDto.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Користувача з ID {vaccinationUpdateDto.UserId} не знайдено");
                }
            }

            vaccination.UserId = vaccinationUpdateDto.UserId;
            vaccination.VaccineName = vaccinationUpdateDto.VaccineName;
            vaccination.VaccinationDate = vaccinationUpdateDto.VaccinationDate;
            vaccination.Manufacturer = vaccinationUpdateDto.Manufacturer;
            vaccination.DoseNumber = vaccinationUpdateDto.DoseNumber;

            await _vaccinationRepository.UpdateAsync(vaccination);
        }

        public async Task DeleteVaccinationAsync(int id)
        {
            var vaccination = await _vaccinationRepository.GetByIdAsync(id);
            if (vaccination == null)
            {
                throw new KeyNotFoundException($"Вакцинацію з ID {id} не знайдено");
            }

            await _vaccinationRepository.DeleteAsync(id);
        }

        private async Task<VaccinationDto> MapToDtoAsync(Vaccination vaccination)
        {
            var user = vaccination.User ?? await _userRepository.GetByIdAsync(vaccination.UserId);

            var dto = new VaccinationDto
            {
                Id = vaccination.Id,
                UserId = vaccination.UserId,
                UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                VaccineName = vaccination.VaccineName,
                VaccinationDate = vaccination.VaccinationDate,
                Manufacturer = vaccination.Manufacturer,
                DoseNumber = vaccination.DoseNumber,
                CreatedAt = vaccination.CreatedAt
            };

            // Додавання документів, якщо вони є
            if (vaccination.Documents != null && vaccination.Documents.Any())
            {
                foreach (var doc in vaccination.Documents)
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
                        RelatedEntityId = vaccination.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }
            else
            {
                // Завантаження документів, якщо вони не були включені
                var documents = await _documentRepository.GetByRelatedEntityIdAsync(vaccination.Id, DocumentType.Vaccination);
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
                        RelatedEntityId = vaccination.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }

            return dto;
        }

        private async Task<IEnumerable<VaccinationDto>> MapToDtosAsync(IEnumerable<Vaccination> vaccinations)
        {
            var result = new List<VaccinationDto>();
            foreach (var vaccination in vaccinations)
            {
                result.Add(await MapToDtoAsync(vaccination));
            }
            return result;
        }
    }
}
