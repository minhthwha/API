using Microsoft.AspNetCore.Mvc;
using ToDoList.DTOs;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // User registration.
        [HttpPost("Register")]
        public async Task<ActionResult<Response<UserRequest>>> Register(UserRequest userRegistration)
        {
            // Get data from UserService.
            var userRegister = await _userService.Register(userRegistration);

            if(userRegister != null)
            {
                return Ok(new Response<UserRequest>
                {
                    Data = userRegistration
                });
            }
            return BadRequest();
        }

        // User log in.
        [HttpPost("LogIn")]
        public async Task<ActionResult<Response<string>>> LogIn(LogInRequest userLogIn)
        {
            // Get data from UserService.
            var userSignIn = await _userService.LogIn(userLogIn);

            if (userSignIn != null)
            {
                return Ok(userSignIn);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
