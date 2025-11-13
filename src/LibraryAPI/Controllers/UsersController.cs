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

        /// <summary>
        /// Update user profile (name and email)
        /// </summary>
        [HttpPut("{id}/profile")]
        public async Task<ActionResult<UserResponseDto>> UpdateProfile(int id, [FromBody] UserUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.UpdateProfileAsync(id, updateDto);

            if (user == null)
            {
                return BadRequest(new { message = "Usuário não encontrado ou email já está em uso" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Update user password
        /// </summary>
        [HttpPut("{id}/password")]
        public async Task<ActionResult> UpdatePassword(int id, [FromBody] UserPasswordUpdateDto passwordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userService.UpdatePasswordAsync(id, passwordDto);

            if (!success)
            {
                return BadRequest(new { message = "Senha atual incorreta ou usuário não encontrado" });
            }

            return Ok(new { message = "Senha atualizada com sucesso" });
        }
    }
}
