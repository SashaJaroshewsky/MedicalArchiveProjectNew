using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class DoctorAccessService : IDoctorAccessService
    {
        private readonly IDoctorAccessRepository _doctorAccessRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorAccessService(
            IDoctorAccessRepository doctorAccessRepository,
            IUserRepository userRepository,
            IDoctorRepository doctorRepository)
        {
            _doctorAccessRepository = doctorAccessRepository;
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<DoctorAccessDto>> GetAllDoctorAccessesAsync()
        {
            var accesses = await _doctorAccessRepository.GetAllAsync();
            return await MapToDtosAsync(accesses);
        }

        public async Task<IEnumerable<DoctorAccessDto>> GetDoctorAccessesByUserIdAsync(int userId)
        {
            var accesses = await _doctorAccessRepository.GetByUserIdAsync(userId);
            return await MapToDtosAsync(accesses);
        }

        public async Task<IEnumerable<DoctorAccessDto>> GetDoctorAccessesByDoctorIdAsync(int doctorId)
        {
            var accesses = await _doctorAccessRepository.GetByDoctorIdAsync(doctorId);
            return await MapToDtosAsync(accesses);
        }

        public async Task<DoctorAccessDto> GetDoctorAccessByIdAsync(int id)
        {
            var access = await _doctorAccessRepository.GetByIdAsync(id);
            if (access == null)
                return null;

            return await MapToDtoAsync(access);
        }

        public async Task<DoctorAccessDto> CreateDoctorAccessAsync(DoctorAccessCreateDto accessCreateDto)
        {
            // Перевірка чи існує користувач
            var user = await _userRepository.GetByIdAsync(accessCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {accessCreateDto.UserId} не знайдено");
            }

            // Перевірка чи існує лікар
            var doctor = await _doctorRepository.GetByIdAsync(accessCreateDto.DoctorId);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"Лікаря з ID {accessCreateDto.DoctorId} не знайдено");
            }

            // Перевірка чи вже існує доступ
            if (await _doctorAccessRepository.AccessExistsAsync(accessCreateDto.UserId, accessCreateDto.DoctorId))
            {
                throw new InvalidOperationException($"Доступ для лікаря з ID {accessCreateDto.DoctorId} до користувача з ID {accessCreateDto.UserId} вже існує");
            }

            var access = new DoctorAccess
            {
                UserId = accessCreateDto.UserId,
                DoctorId = accessCreateDto.DoctorId,
                ExpiresAt = accessCreateDto.ExpiresAt,
                User = user,
                Doctor = doctor
            };

            await _doctorAccessRepository.AddAsync(access);
            return await MapToDtoAsync(access);
        }

        public async Task DeleteDoctorAccessAsync(int id)
        {
            var access = await _doctorAccessRepository.GetByIdAsync(id);
            if (access == null)
            {
                throw new KeyNotFoundException($"Доступ з ID {id} не знайдено");
            }

            await _doctorAccessRepository.DeleteAsync(id);
        }

        private async Task<DoctorAccessDto> MapToDtoAsync(DoctorAccess access)
        {
            var user = access.User ?? await _userRepository.GetByIdAsync(access.UserId);
            var doctor = access.Doctor ?? await _doctorRepository.GetByIdAsync(access.DoctorId);

            return new DoctorAccessDto
            {
                Id = access.Id,
                UserId = access.UserId,
                DoctorId = access.DoctorId,
                UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                DoctorFullName = doctor != null ? $"{doctor.LastName} {doctor.FirstName} {doctor.MiddleName}" : "Невідомо",
                GrantedAt = access.GrantedAt,
                ExpiresAt = access.ExpiresAt
            };
        }

        private async Task<IEnumerable<DoctorAccessDto>> MapToDtosAsync(IEnumerable<DoctorAccess> accesses)
        {
            var result = new List<DoctorAccessDto>();
            foreach (var access in accesses)
            {
                result.Add(await MapToDtoAsync(access));
            }
            return result;
        }
    }
}
