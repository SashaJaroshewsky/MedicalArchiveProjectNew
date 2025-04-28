using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IDoctorAppointmentRepository : IBaseRepository<DoctorAppointment>
    {
        Task<IEnumerable<DoctorAppointment>> GetByUserIdAsync(int userId);
        Task<DoctorAppointment?> GetWithDocumentsAsync(int appointmentId);
    }
}
