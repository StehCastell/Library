namespace LibraryAPI.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Theme { get; set; } = "light";
        public string? ProfileImage { get; set; }
    }
}
