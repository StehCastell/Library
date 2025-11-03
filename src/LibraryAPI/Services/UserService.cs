using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using BCrypt.Net;

namespace LibraryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> RegisterAsync(UserRegisterDto registerDto)
        {
            // Verificar se o email já existe
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                return null;
            }

            // Criar novo usuário
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.Now
            };

            var userCriado = await _userRepository.CreateAsync(user);

            return new UserResponseDto
            {
                Id = userCriado.Id,
                Name = userCriado.Name,
                Email = userCriado.Email,
                CreatedAt = userCriado.CreatedAt
            };
        }

        public async Task<UserResponseDto?> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            // Verificar password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
