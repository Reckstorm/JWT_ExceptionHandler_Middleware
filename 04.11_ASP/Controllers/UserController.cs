using _04._11_ASP.Exceptions;
using _04._11_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _04._11_ASP.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private Context _context;
        private IConfiguration _config;

        public UserController(Context context, IConfiguration configuration)
        {
            this._context = context;
            this._config = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Post([FromForm] UserDto user)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", $"{user.FirstName}_{user.LastName}_avatarImg.{Path.GetFileName(user.AvatarImg.FileName)}");
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    user.AvatarImg.CopyTo(stream);
                }
                UserModel temp = new UserModel();
                temp.FirstName = user.FirstName.Trim();
                temp.LastName = user.LastName.Trim();
                temp.Email = user.Email.Trim();
                temp.PasswordHash = passwordHash;
                temp.AvatarImg = user.AvatarImg;

                _context.UserModels.Attach(temp).State = EntityState.Added;
                _context.SaveChanges();

                return Ok(temp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromForm] UserDto user)
        {
            try
            {
                UserModel temp = _context.UserModels.FirstOrDefault(u => u.Email.Equals(user.Email));
                if(temp == null) return StatusCode(401);
                if(!BCrypt.Net.BCrypt.Verify(user.Password, temp.PasswordHash)) return StatusCode(401);
                string token = CreateJWT(temp);
                return Ok(token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string CreateJWT(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));

            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: cred
                );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return(jwt);
        }
    }
}
