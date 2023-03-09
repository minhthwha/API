using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ToDoListDbContext _dbContext;
        private readonly IMapper _mapper;

        // Variable declaration for database using.
        public UserService(IConfiguration configuration, ToDoListDbContext dbContext, IMapper mapper)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Response<UserRequest>> Register(UserRequest request)
        {
            // Find the username which already in database.
            var query = await _dbContext.Users.FirstOrDefaultAsync(t => t.UserName == request.UserName);

            if (query != null)
            {
                return null;
            }
            else
            {
                var userMap = _mapper.Map<User>(request);

                // Add user.
                _dbContext.Users.Add(userMap);
                _dbContext.SaveChanges();

                return new Response<UserRequest>
                {
                    Data = request
                };
            }
        }

        public async Task<Response<string>> LogIn(LogInRequest request)
        {
            // Check if username existed.
            var user = await _dbContext.Users.FirstOrDefaultAsync(t => t.UserName == request.UserName);

            if (user != null)
            {
                // Verify hash password in database if username found.
                bool verify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

                if (verify == true)
                {
                    // Create claims details based on the user informations.
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim("Name", user.Name),
                        new Claim("Email", user.Email),
                        new Claim("PhoneNumber", user.PhoneNumber),
                        new Claim("Address", user.Address),
                        new Claim("UserName", user.UserName),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var logIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var jwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: logIn);

                    return new Response<string>
                    {
                        Data = new JwtSecurityTokenHandler().WriteToken(jwt)
                    };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
