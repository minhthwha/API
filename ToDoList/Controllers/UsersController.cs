using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize] // Thêm thuộc tính ủy quyền => Tất cả các API trong controller đều được bảo mật bằng token.
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ToDoListDbContext _dbContext;

        // Khai báo biến để dùng database.
        public UsersController(ToDoListDbContext dbContext)
        {
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
                Password = userRegistration.Password,
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
            var query = _dbContext.Users.FirstOrDefault(t => t.UserName == userLogIn.UserName && t.Password == userLogIn.Password);
            if(query == null)
            {
                return BadRequest("Tên người dùng hoặc mật khẩu không trùng khớp!");
            }
            else
            {
                return Ok("Đăng nhập thành công!");
            }
        }
    }
}
