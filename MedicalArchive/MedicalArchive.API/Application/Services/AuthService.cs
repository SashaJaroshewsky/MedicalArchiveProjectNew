using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalArchive.API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IDoctorRepository doctorRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> AuthenticateUserAsync(AuthDto authDto)
        {
            var user = await _userRepository.GetByEmailAsync(authDto.Email);
            if (user == null)
            {
                return null;
            }

            // В реальній системі тут має бути перевірка хешу паролю
            // Наприклад: if (!BCrypt.Verify(authDto.Password, user.PasswordHash)) return null;

            // Спрощена перевірка для демонстрації
            if (user.PasswordHash != authDto.Password)
            {
                return null;
            }

            var token = await GenerateJwtTokenAsync(user.Id, user.Email, false);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.LastName} {user.FirstName} {user.MiddleName}",
                Token = token,
                IsDoctor = false
            };
        }

        public async Task<AuthResponseDto> AuthenticateDoctorAsync(AuthDto authDto)
        {
            var doctor = await _doctorRepository.GetByEmailAsync(authDto.Email);
            if (doctor == null)
            {
                return null;
            }

            // В реальній системі тут має бути перевірка хешу паролю
            // Наприклад: if (!BCrypt.Verify(authDto.Password, doctor.PasswordHash)) return null;

            // Спрощена перевірка для демонстрації
            if (doctor.PasswordHash != authDto.Password)
            {
                return null;
            }

            var token = await GenerateJwtTokenAsync(doctor.Id, doctor.Email, true);

            return new AuthResponseDto
            {
                Id = doctor.Id,
                Email = doctor.Email,
                FullName = $"{doctor.LastName} {doctor.FirstName} {doctor.MiddleName}",
                Token = token,
                IsDoctor = true
            };
        }

        public async Task<string> GenerateJwtTokenAsync(int userId, string email, bool isDoctor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "DefaultSecretKeyForDevelopment12345"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, isDoctor ? "Doctor" : "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"] ?? "DefaultIssuer",
                audience: _configuration["JWT:Audience"] ?? "DefaultAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
