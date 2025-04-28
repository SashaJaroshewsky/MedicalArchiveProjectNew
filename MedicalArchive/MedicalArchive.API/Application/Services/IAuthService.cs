using MedicalArchive.API.Application.DTOs;

namespace MedicalArchive.API.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateUserAsync(AuthDto authDto);
        Task<AuthResponseDto> AuthenticateDoctorAsync(AuthDto authDto);
        Task<string> GenerateJwtTokenAsync(int userId, string email, bool isDoctor);
    }
}
