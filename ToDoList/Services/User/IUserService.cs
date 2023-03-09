using ToDoList.DTOs;

namespace ToDoList.Services
{
    public interface IUserService
    {
        Task<Response<UserRequest>> Register(UserRequest request);
        Task<Response<string>> LogIn(LogInRequest request);
    }
}
