using MedicalArchive.API.Domain.Models;

namespace MedicalArchive.API.Infrastructure.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithDoctorAccessesAsync(int userId);
        Task<User?> GetUserWithMedicalDocumentsAsync(int userId);
        Task<bool> UserExistsAsync(string email);
    }
}
