using LibraryAPI.DTOs;

namespace LibraryAPI.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterAsync(UserRegisterDto registerDto);
        Task<UserResponseDto?> LoginAsync(UserLoginDto loginDto);
        Task<UserResponseDto?> GetByIdAsync(int id);
    }
}
