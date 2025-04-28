using MedicalArchive.API.Application.DTOs.ReferralDTOs;
using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _referralRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMedicalDocumentRepository _documentRepository;

        public ReferralService(
            IReferralRepository referralRepository,
            IUserRepository userRepository,
            IMedicalDocumentRepository documentRepository)
        {
            _referralRepository = referralRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
        }

        public async Task<IEnumerable<ReferralDto>> GetAllReferralsAsync()
        {
            var referrals = await _referralRepository.GetAllAsync();
            return await MapToDtosAsync(referrals);
        }

        public async Task<IEnumerable<ReferralDto>> GetReferralsByUserIdAsync(int userId)
        {
            var referrals = await _referralRepository.GetByUserIdAsync(userId);
            return await MapToDtosAsync(referrals);
        }

        public async Task<ReferralDto> GetReferralByIdAsync(int id)
        {
            var referral = await _referralRepository.GetWithDocumentsAsync(id);
            if (referral == null)
                return null;

            return await MapToDtoAsync(referral);
        }

        public async Task<ReferralDto> CreateReferralAsync(ReferralCreateDto referralCreateDto)
        {
            // Перевірка чи існує користувач
            var user = await _userRepository.GetByIdAsync(referralCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {referralCreateDto.UserId} не знайдено");
            }

            var referral = new Referral
            {
                UserId = referralCreateDto.UserId,
                IssueDate = referralCreateDto.IssueDate,
                ExpirationDate = referralCreateDto.ExpirationDate,
                ReferralType = referralCreateDto.ReferralType,
                ReferralNumber = referralCreateDto.ReferralNumber,
                User = user
            };

            await _referralRepository.AddAsync(referral);
            return await MapToDtoAsync(referral);
        }

        public async Task UpdateReferralAsync(int id, ReferralCreateDto referralUpdateDto)
        {
            var referral = await _referralRepository.GetByIdAsync(id);
            if (referral == null)
            {
                throw new KeyNotFoundException($"Направлення з ID {id} не знайдено");
            }

            // Перевірка чи користувач змінився і чи існує новий користувач
            if (referral.UserId != referralUpdateDto.UserId)
            {
                var user = await _userRepository.GetByIdAsync(referralUpdateDto.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Користувача з ID {referralUpdateDto.UserId} не знайдено");
                }
            }

            referral.UserId = referralUpdateDto.UserId;
            referral.IssueDate = referralUpdateDto.IssueDate;
            referral.ExpirationDate = referralUpdateDto.ExpirationDate;
            referral.ReferralType = referralUpdateDto.ReferralType;
            referral.ReferralNumber = referralUpdateDto.ReferralNumber;

            await _referralRepository.UpdateAsync(referral);
        }

        public async Task DeleteReferralAsync(int id)
        {
            var referral = await _referralRepository.GetByIdAsync(id);
            if (referral == null)
            {
                throw new KeyNotFoundException($"Направлення з ID {id} не знайдено");
            }

            await _referralRepository.DeleteAsync(id);
        }

        private async Task<ReferralDto> MapToDtoAsync(Referral referral)
        {
            var user = referral.User ?? await _userRepository.GetByIdAsync(referral.UserId);

            var dto = new ReferralDto
            {
                Id = referral.Id,
                UserId = referral.UserId,
                UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                IssueDate = referral.IssueDate,
                ExpirationDate = referral.ExpirationDate,
                ReferralType = referral.ReferralType,
                ReferralNumber = referral.ReferralNumber,
                CreatedAt = referral.CreatedAt
            };

            // Додавання документів, якщо вони є
            if (referral.Documents != null && referral.Documents.Any())
            {
                foreach (var doc in referral.Documents)
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
                        RelatedEntityId = referral.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }
            else
            {
                // Завантаження документів, якщо вони не були включені
                var documents = await _documentRepository.GetByRelatedEntityIdAsync(referral.Id, DocumentType.Referral);
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
                        RelatedEntityId = referral.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }

            return dto;
        }

        private async Task<IEnumerable<ReferralDto>> MapToDtosAsync(IEnumerable<Referral> referrals)
        {
            var result = new List<ReferralDto>();
            foreach (var referral in referrals)
            {
                result.Add(await MapToDtoAsync(referral));
            }
            return result;
        }
    }
}
