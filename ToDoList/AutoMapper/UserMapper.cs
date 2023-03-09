using AutoMapper;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserRequest>();
            CreateMap<UserRequest, User>();
            CreateMap<LogInRequest, User>();
            CreateMap<User, LogInRequest>();
        }
    }
}
