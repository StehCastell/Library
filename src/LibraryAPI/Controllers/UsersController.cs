using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registrar um novo usuário
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.RegisterAsync(registerDto);

            if (user == null)
            {
                return BadRequest(new { message = "Email já está cadastrado" });
            }

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.LoginAsync(loginDto);

            if (user == null)
            {
                return Unauthorized(new { message = "Email ou password incorretos" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuário not found" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Update user theme
        /// </summary>
        [HttpPut("{id}/theme")]
        public async Task<ActionResult> UpdateTheme(int id, [FromBody] UserThemeDto themeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userService.UpdateThemeAsync(id, themeDto.Theme);

            if (!success)
            {
                return NotFound(new { message = "Usuário not found" });
            }

            return Ok(new { message = "Theme updated successfully" });
        }
    }
}
