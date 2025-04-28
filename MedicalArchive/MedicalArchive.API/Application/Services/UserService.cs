using MedicalArchive.API.Application.DTOs.UserDTOs;
using MedicalArchive.API.Domain.Models;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            // Перевірка чи існує користувач з таким email
            if (await _userRepository.UserExistsAsync(userCreateDto.Email))
            {
                throw new InvalidOperationException($"Користувач з email {userCreateDto.Email} вже існує");
            }

            // В реальній системі тут має бути хешування паролю
            // passwordHash = BCrypt.HashPassword(userCreateDto.Password);

            var user = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                MiddleName = userCreateDto.MiddleName,
                DateOfBirth = userCreateDto.DateOfBirth,
                Gender = userCreateDto.Gender,
                Email = userCreateDto.Email,
                PhoneNumber = userCreateDto.PhoneNumber,
                Address = userCreateDto.Address,
                PasswordHash = userCreateDto.Password // В реальній системі тут має бути хеш
            };

            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        public async Task UpdateUserAsync(int id, UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {id} не знайдено");
            }

            // Перевірка унікальності email, якщо він змінився
            if (user.Email != userDto.Email && await _userRepository.UserExistsAsync(userDto.Email))
            {
                throw new InvalidOperationException($"Користувач з email {userDto.Email} вже існує");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.MiddleName = userDto.MiddleName;
            user.DateOfBirth = userDto.DateOfBirth;
            user.Gender = userDto.Gender;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Address = userDto.Address;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Користувача з ID {id} не знайдено");
            }

            await _userRepository.DeleteAsync(id);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
