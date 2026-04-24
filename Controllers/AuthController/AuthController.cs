using ecomServer.Data;
using ecomServer.DTO.AuthDTO;
using ecomServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ecomServer.Utils;

namespace ecomServer.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EcomDbContext _db;
        private readonly JwtUtil _jwtUtils;
        private readonly PasswordHashUtil _PassHash;
        public AuthController(EcomDbContext context, JwtUtil jwtUtils,PasswordHashUtil passwordHash)
        {
            _db = context;
            _jwtUtils = jwtUtils;
            _PassHash = passwordHash;
        }

        // REGISTER
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_db.Users.Any(x => x.UserName == dto.UserName))
                return BadRequest("Username already exists");

            _PassHash.CreatePasswordHash(dto.Password, out string hash, out string salt);

            var user = new User
            {
                UserFirstName = dto.FirstName,
                UserLastName = dto.LastName,
                UserName = dto.UserName,
                UserEmail = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserTypeId = 2, // default user
                IsActive = true
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok("User registered successfully");
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == dto.UserName && x.IsActive == true);
            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!_PassHash.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid credentials");

            user.UserType = _db.UserTypes.Find(user.UserTypeId); 

            var token = _jwtUtils.GenerateJwtToken(user);
            return Ok(new { token });
        }

    }
}
