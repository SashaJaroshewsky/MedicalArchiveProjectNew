using MedicalArchive.API.Application.DTOs.DoctorAppointmentDTOs;
using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class DoctorAppointmentService : IDoctorAppointmentService
    {
        private readonly IDoctorAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMedicalDocumentRepository _documentRepository;

        public DoctorAppointmentService(
            IDoctorAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            IMedicalDocumentRepository documentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
        }

        public async Task<IEnumerable<DoctorAppointmentDto>> GetAllDoctorAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return await MapToDtosAsync(appointments);
        }

        public async Task<IEnumerable<DoctorAppointmentDto>> GetDoctorAppointmentsByUserIdAsync(int userId)
        {
            var appointments = await _appointmentRepository.GetByUserIdAsync(userId);
            return await MapToDtosAsync(appointments);
        }

        public async Task<DoctorAppointmentDto> GetDoctorAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetWithDocumentsAsync(id);
            if (appointment == null)
                return null;

            return await MapToDtoAsync(appointment);
        }

        public async Task<DoctorAppointmentDto> CreateDoctorAppointmentAsync(DoctorAppointmentCreateDto appointmentCreateDto)
        {
            // Перевірка чи існує користувач
            var user = await _userRepository.GetByIdAsync(appointmentCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {appointmentCreateDto.UserId} не знайдено");
            }

            var appointment = new DoctorAppointment
            {
                UserId = appointmentCreateDto.UserId,
                AppointmentDate = appointmentCreateDto.AppointmentDate,
                DoctorInfo = appointmentCreateDto.DoctorInfo,
                Complaints = appointmentCreateDto.Complaints,
                ProceduresDescription = appointmentCreateDto.ProceduresDescription,
                Diagnosis = appointmentCreateDto.Diagnosis,
                User = user
            };

            await _appointmentRepository.AddAsync(appointment);
            return await MapToDtoAsync(appointment);
        }

        public async Task UpdateDoctorAppointmentAsync(int id, DoctorAppointmentCreateDto appointmentUpdateDto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Прийом з ID {id} не знайдено");
            }

            // Перевірка чи користувач змінився і чи існує новий користувач
            if (appointment.UserId != appointmentUpdateDto.UserId)
            {
                var user = await _userRepository.GetByIdAsync(appointmentUpdateDto.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Користувача з ID {appointmentUpdateDto.UserId} не знайдено");
                }
            }

            appointment.UserId = appointmentUpdateDto.UserId;
            appointment.AppointmentDate = appointmentUpdateDto.AppointmentDate;
            appointment.DoctorInfo = appointmentUpdateDto.DoctorInfo;
            appointment.Complaints = appointmentUpdateDto.Complaints;
            appointment.ProceduresDescription = appointmentUpdateDto.ProceduresDescription;
            appointment.Diagnosis = appointmentUpdateDto.Diagnosis;

            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task DeleteDoctorAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Прийом з ID {id} не знайдено");
            }

            await _appointmentRepository.DeleteAsync(id);
        }

        private async Task<DoctorAppointmentDto> MapToDtoAsync(DoctorAppointment appointment)
        {
            var user = appointment.User ?? await _userRepository.GetByIdAsync(appointment.UserId);

            var dto = new DoctorAppointmentDto
            {
                Id = appointment.Id,
                UserId = appointment.UserId,
                UserFullName = user != null ? $"{user.LastName} {user.FirstName} {user.MiddleName}" : "Невідомо",
                AppointmentDate = appointment.AppointmentDate,
                DoctorInfo = appointment.DoctorInfo,
                Complaints = appointment.Complaints,
                ProceduresDescription = appointment.ProceduresDescription,
                Diagnosis = appointment.Diagnosis,
                CreatedAt = appointment.CreatedAt
            };

            // Додавання документів, якщо вони є
            if (appointment.Documents != null && appointment.Documents.Any())
            {
                foreach (var doc in appointment.Documents)
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
                        RelatedEntityId = appointment.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }
            else
            {
                // Завантаження документів, якщо вони не були включені
                var documents = await _documentRepository.GetByRelatedEntityIdAsync(appointment.Id, DocumentType.DoctorAppointment);
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
                        RelatedEntityId = appointment.Id,
                        CreatedAt = doc.CreatedAt
                    });
                }
            }

            return dto;
        }

        private async Task<IEnumerable<DoctorAppointmentDto>> MapToDtosAsync(IEnumerable<DoctorAppointment> appointments)
        {
            var result = new List<DoctorAppointmentDto>();
            foreach (var appointment in appointments)
            {
                result.Add(await MapToDtoAsync(appointment));
            }
            return result;
        }
    }
}
