using MedicalArchive.API.Application.DTOs.DoctorDTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return doctors.Select(MapToDto);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            return doctor != null ? MapToDto(doctor) : null;
        }

        public async Task<DoctorDto> GetDoctorByEmailAsync(string email)
        {
            var doctor = await _doctorRepository.GetByEmailAsync(email);
            return doctor != null ? MapToDto(doctor) : null;
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto doctorCreateDto)
        {
            // Перевірка чи існує лікар з таким email
            if (await _doctorRepository.DoctorExistsAsync(doctorCreateDto.Email))
            {
                throw new InvalidOperationException($"Лікар з email {doctorCreateDto.Email} вже існує");
            }

            // В реальній системі тут має бути хешування паролю
            // passwordHash = BCrypt.HashPassword(doctorCreateDto.Password);

            var doctor = new Doctor
            {
                FirstName = doctorCreateDto.FirstName,
                LastName = doctorCreateDto.LastName,
                MiddleName = doctorCreateDto.MiddleName,
                Specialization = doctorCreateDto.Specialization,
                MedicalInstitution = doctorCreateDto.MedicalInstitution,
                Email = doctorCreateDto.Email,
                PhoneNumber = doctorCreateDto.PhoneNumber,
                PasswordHash = doctorCreateDto.Password // В реальній системі тут має бути хеш
            };

            await _doctorRepository.AddAsync(doctor);
            return MapToDto(doctor);
        }

        public async Task UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"Лікаря з ID {id} не знайдено");
            }

            // Перевірка унікальності email, якщо він змінився
            if (doctor.Email != doctorDto.Email && await _doctorRepository.DoctorExistsAsync(doctorDto.Email))
            {
                throw new InvalidOperationException($"Лікар з email {doctorDto.Email} вже існує");
            }

            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.MiddleName = doctorDto.MiddleName;
            doctor.Specialization = doctorDto.Specialization;
            doctor.MedicalInstitution = doctorDto.MedicalInstitution;
            doctor.Email = doctorDto.Email;
            doctor.PhoneNumber = doctorDto.PhoneNumber;

            await _doctorRepository.UpdateAsync(doctor);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"Лікаря з ID {id} не знайдено");
            }

            await _doctorRepository.DeleteAsync(id);
        }

        private DoctorDto MapToDto(Doctor doctor)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                MiddleName = doctor.MiddleName,
                Specialization = doctor.Specialization,
                MedicalInstitution = doctor.MedicalInstitution,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                CreatedAt = doctor.CreatedAt
            };
        }
    }
}
