using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ToDoListDbContext _dbContext;

        // Khai báo biến để dùng database.
        public UsersController(IConfiguration configuration, ToDoListDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        // Đăng kí người dùng.
        [HttpPost("Register")]
        public IActionResult Registration(UserRequest userRegistration)
        {
            // Khai báo người dùng mới.
            var user = new User
            {
                Name = userRegistration.Name,
                Email = userRegistration.Email,
                PhoneNumber = userRegistration.PhoneNumber,
                Address = userRegistration.Address,
                UserName = userRegistration.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegistration.Password)
            };            

            // Gán query bằng các giá trị username đã có trong database.
            var query = _dbContext.Users.FirstOrDefault(t => t.UserName == userRegistration.UserName);
            if(query != null)
            {
                return BadRequest("Tên người dùng đã tồn tại!");
            }
            else
            {
                // Thêm người dùng.
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return (Ok("Đăng kí thành công!"));
            }
        }

        // Đăng nhập tài khoản.
        [HttpPost("LogIn")]
        public IActionResult LogIn(LogInRequest userLogIn)
        {
            // Kiểm tra tên đăng nhập có tồn tại không.
            var user = _dbContext.Users.FirstOrDefault(t => t.UserName == userLogIn.UserName);
            if(user != null)
            {
                // Nếu tên đăng nhập có trong database thì verify mật khẩu được mã hóa trong database. 
                bool verify = BCrypt.Net.BCrypt.Verify(userLogIn.Password, user.Password);
                if (verify == true)
                {
                    // Tạo chi tiết claims dựa trên thông tin của người dùng.
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
                return BadRequest("Tên người dùng hoặc mật khẩu không trùng khớp!");
            }          
        }
    }
}
