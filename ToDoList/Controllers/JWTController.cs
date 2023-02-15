using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.DTOs;
using ToDoList.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ToDoList.Controllers
{
    [Route("api/JWT")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ToDoListDbContext _dbContext;

        public JWTController(IConfiguration configuration, ToDoListDbContext toDoListDbContext) {
            _configuration = configuration;
            _dbContext = toDoListDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(User _user)
        {
            if (_user != null && _user.UserName != null && _user.Password != null)
            {
                var user = await GetUser(_user.UserName, _user.Password);

                // Kiểm tra tính hợp lệ của tên đăng nhập và mật khẩu
                if (user != null)
                {
                    // Create claims details based on the user informations.
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Name", user.Name),
                        new Claim("Email", user.Email),
                        new Claim("PhoneNumber", user.PhoneNumber),
                        new Claim("Address", user.Address),
                        new Claim("UserName", user.UserName),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var logIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var jwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, signingCredentials: logIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
                }
                else
                {
                    return BadRequest("Thông tin không hợp lệ!");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string userName, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(t => t.UserName == userName && t.Password == password);
        }
    }
}
