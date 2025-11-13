using LibraryAPI.DTOs;

namespace LibraryAPI.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterAsync(UserRegisterDto registerDto);
        Task<UserResponseDto?> LoginAsync(UserLoginDto loginDto);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<bool> UpdateThemeAsync(int userId, string theme);
        Task<UserResponseDto?> UpdateProfileAsync(int userId, UserUpdateDto updateDto);
        Task<bool> UpdatePasswordAsync(int userId, UserPasswordUpdateDto passwordDto);
    }
}
